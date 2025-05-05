using AutoMapper;
using CustomerManagement.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using CustomerManagement.Application.Interfaces;
using CustomerManagement.Common.DTOs;
using CustomerManagement.Common.Helpers;
using CustomerManagement.Web.Models;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace CustomerManagement.Web.Controllers;
public class AppraisalsController : BaseController
{
    private readonly IAppraisalService _appraisalService;
    private readonly IGenericService<EmployeeDto> _employeeService;
    private readonly IMapper _mapper;
    public AppraisalsController(IAppraisalService appraisalService, IGenericService<EmployeeDto> employeeService, IMapper mapper)
    {
        _appraisalService = appraisalService;
        _employeeService = employeeService;
        _mapper = mapper;
    }

    #region Workflow
    /// <summary>
    /// Index display Appraisal related to login employee
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> AppIndex(int id = 0)
    {
        //string? userIdQry = Request.Query["id"];
        //if (string.IsNullOrWhiteSpace(userIdQry) || Convert.ToInt32(userIdQry) == 0)
        //{
        //    if (id == 0) return NotFound(); else userIdQry = Convert.ToString(id);
        //}
        if (id == 0 || id == null) return NotFound();
        var employee = await _employeeService.GetByIdAsync(id);
        if (employee == null)
        {
            return NotFound();
        }
        //Get User Data from Cookie
        string userName = string.Empty;
        int? roleID = 0, userID = 0, managerID = 0;

        //Set User Data in Cookie
        //SetUserInfo(userIdQry);
        SetGetUserInfo(employee, out userName, out roleID, out userID, out managerID);
        userID = Convert.ToInt32(employee?.EmployeeId);
        userName = employee?.EmployeeName;
        roleID = Convert.ToInt32(employee?.RoleId);
        managerID = Convert.ToInt32(employee?.ManagerId);
        //GetUserInfo(ref userName, ref roleID, ref userID, ref managerID);
        SearchInParamDto param = new SearchInParamDto();
        param.DBAction = "GetAppraisalByUId";
        param.LoginId = userID;
        param.RoleId = roleID;
        var appraisals = await _appraisalService.GetAppraisalByUIdAsync(param);
        return View("Index", appraisals); // If using Razor View
    }
    /// <summary>
    /// Get Appraisal details based on AppraisalId
    /// Help to display Appraisal Details, Histories and Handle Action
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> AppDetails(int id = 0)
    {
        string strView = "AppDetails";
        var detailVM = new DetailViewModel();
        try
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            SearchInParamDto param = new SearchInParamDto();
            param.DBAction = "GetAppraisalDetailById";
            param.AppraisalId = id;
            var appraisalDetail = await _appraisalService.GetAppraisalDetailsAsync(param);
            if (appraisalDetail != null)
            {
                if (appraisalDetail.AppraisalDto != null) detailVM.AppraisalDto = appraisalDetail.AppraisalDto;
                if (appraisalDetail.AppraisalTaskDto != null) detailVM.AppraisalTaskDto = appraisalDetail.AppraisalTaskDto;
                if (appraisalDetail.AppraisalTaskHistoryDtos != null) detailVM.AppraisalTaskHistoryDtos = appraisalDetail.AppraisalTaskHistoryDtos;
            }
            //Get User Data from Cookie
            string userName = string.Empty;
            int? roleID = 0, userID = 0, managerID = 0;
            GetUserInfo(ref userName, ref roleID, ref userID, ref managerID);

            if (userID.HasValue && userID > 0)
            {
                detailVM.AppraisalDto.LoginId = userID;
                detailVM.AppraisalDto.AssignBy = userID;
                detailVM.AppraisalDto.AssignTo = managerID;
                detailVM.AppraisalTaskDto.LoginId = userID;
                detailVM.AppraisalTaskDto.AssignBy = userID;
                detailVM.AppraisalTaskDto.AssignTo = managerID;
            }
            if (roleID.HasValue && roleID > 0) { detailVM.AppraisalTaskDto.RoleId = roleID; }
            if (detailVM.AppraisalTaskDto != null && !string.IsNullOrWhiteSpace(detailVM.AppraisalTaskDto.FormType)) strView = detailVM.AppraisalTaskDto.FormType;
            ////When Appraisal Initialize - No Appraisal Data in DB
            //if (detailVM.AppraisalTaskDto == null || detailVM.AppraisalTaskDto.ActionId == null)
            //{
            //    if (detailVM.AppraisalTaskDto.ActionId == null) { detailVM.AppraisalTaskDto.ActionId = (int)CommonAppraisal.UserAction.Phase_1_Emp_New; }
            //    if (!string.IsNullOrWhiteSpace(detailVM.AppraisalTaskDto.FormType)) strView = detailVM.AppraisalTaskDto.FormType;
            //}

            return View(detailVM);
        }
        catch (Exception ex)
        {
            return View(strView, detailVM);
            //throw;
        }

    }

    #region Workflow-Action
    /// <summary>
    /// Perform all workflow submittion on this method
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> SubmitAppraisal(AppraisalDto dto)
    {
        // Only keep fields that are being submitted via the form
        var keepFields = new[] { "DBAction", "RoleId", "AssignBy", "AssignTo", "LoginId" };
        if (dto.DBAction == "Phase1EmpToMngAssign") keepFields = new[] { "DBAction", "RoleId", "AssignBy", "LoginId", "Title", "Message", "Weight" };
        else if (dto.DBAction == "Phase2EmpToMngAssign") keepFields = new[] { "DBAction", "RoleId", "AssignBy", "LoginId", "Achievement" };
        else if (dto.DBAction == "Phase3EmpToMngAssign") keepFields = new[] { "DBAction", "RoleId", "AssignBy", "LoginId", "EmployeeScore", "EmployeeComment" };
        else if (dto.DBAction == "Phase4EmpToMngAssign") keepFields = new[] { "DBAction", "RoleId", "AssignBy", "LoginId", "EmployeeComment" };
        else if (dto.DBAction == "Phase3MngToEmpApprove") keepFields = new[] { "DBAction", "RoleId", "AssignBy", "LoginId", "ManagerScore", "ManagerComment" };
        else if (dto.DBAction == "Phase1MngToEmpReject" || dto.DBAction == "Phase1MngToEmpApprove"
            || dto.DBAction == "Phase2MngToEmpReject" || dto.DBAction == "Phase2MngToEmpApprove" || dto.DBAction == "Phase3MngToEmpReject"
            || dto.DBAction == "Phase4MngToEmpReject" || dto.DBAction == "Phase4MngToEmpApprove") keepFields = new[] { "DBAction", "RoleId", "AssignBy", "LoginId", "ManagerComment" };


        foreach (var key in ModelState.Keys.ToList())
        {
            Console.WriteLine(key);
            if (!keepFields.Contains(key))
            {
                ModelState.Remove(key);
            }
        }
        if (!ModelState.IsValid) return RedirectToAction(nameof(AppDetails), new { id = dto.AppraisalId });
        var appraisal = await _appraisalService.AddAppraisalWFAsync(dto);
        return RedirectToAction(nameof(AppDetails), new { id = appraisal.AppraisalId });
    }
    //[HttpPost]
    //public async Task<IActionResult> Phase1Reject(AppraisalDto dto)
    //{
    //    if (!ModelState.IsValid) return View(dto);
    //    await _appraisalService.AddAppraisalAsync(dto);
    //    return RedirectToAction(nameof(Index));
    //}
    //[HttpPost]
    //public async Task<IActionResult> Phase1Approve(AppraisalDto dto)
    //{
    //    if (!ModelState.IsValid) return View(dto);
    //    await _appraisalService.AddAppraisalAsync(dto);
    //    return RedirectToAction(nameof(Index));
    //}
    /// <summary>
    /// Appraisal - Phase 1 - Performance Ojection - Submit
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<JsonResult> Phase1NewJ([FromBody] AppraisalDto model)
    {
        try
        {
            if (model == null)
                return Json(new { success = false, message = "Model is null" });

            // Only keep fields that are being submitted via the form
            var keepFields = new[] { "DBAction", "RoleId", "AssignBy", "AssignTo", "LoginId", "Title", "Message", "Weight" };

            foreach (var key in ModelState.Keys.ToList())
            {
                Console.WriteLine(key);
                if (!keepFields.Contains(key))
                {
                    ModelState.Remove(key);
                }
            }

            // Validate the model using server-side validation
            if (!ModelState.IsValid)
            {
                // If invalid, gather errors and return a JSON response.
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                var errors2 = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                    );

                return Json(new { success = false, errors, errors2 });
            }

            var appraisal = await _appraisalService.AddAppraisalWFAsync(model);
            return Json(new { success = true, message = "Appraisal saved successfully", appId = appraisal.AppraisalId });
            // Create your entity from the view model, for example:
            // var entity = new AppraisalDto
            // {
            //     Title = model.Title,
            //     Message = model.Message,
            //     Weight = model.Weight,
            //     AppraisalId = model.AppraisalId,
            //     DBAction = model.DBAction,
            //     RoleId = !string.IsNullOrWhiteSpace(model.RoleId) ? Convert.ToInt32(model.RoleId) : 0 ,
            //     AssignBy = !string.IsNullOrWhiteSpace(model.AssignBy) ? Convert.ToInt32(model.AssignBy) : 0,
            //     AssignTo = !string.IsNullOrWhiteSpace(model.AssignTo) ? Convert.ToInt32(model.AssignTo) : 0,
            //     ActionId = model.ActionId,
            //     // Set any additional properties if needed.
            // };
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }
    /// <summary>
    /// Appraisal - Phase 1 - Performance Ojection - Reject
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>

    [HttpPost]
    public async Task<JsonResult> Phase1RejectJ([FromBody] AppraisalDto model)
    {
        try
        {
            if (model == null)
                return Json(new { success = false, message = "Model is null" });

            // Only keep fields that are being submitted via the form
            var keepFields = new[] { "DBAction", "RoleId", "AssignBy", "AssignTo", "LoginId", "ManagerComment" };

            foreach (var key in ModelState.Keys.ToList())
            {
                Console.WriteLine(key);
                if (!keepFields.Contains(key))
                {
                    ModelState.Remove(key);
                }
            }

            // Validate the model using server-side validation
            if (!ModelState.IsValid)
            {
                // If invalid, gather errors and return a JSON response.
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                var errors2 = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                    );

                return Json(new { success = false, errors, errors2 });
            }

            var appraisal = await _appraisalService.AddAppraisalWFAsync(model);
            return Json(new { success = true, message = "Appraisal saved successfully", appId = appraisal.AppraisalId });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }
    /// <summary>
    /// Appraisal - Phase 1 - Performance Ojection - Approve
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<JsonResult> Phase1Approve([FromBody] AppraisalDto model)
    {
        try
        {
            if (model == null)
                return Json(new { success = false, message = "Model is null" });

            // Only keep fields that are being submitted via the form
            var keepFields = new[] { "DBAction", "RoleId", "AssignBy", "AssignTo", "LoginId", "ManagerComment" };

            foreach (var key in ModelState.Keys.ToList())
            {
                Console.WriteLine(key);
                if (!keepFields.Contains(key))
                {
                    ModelState.Remove(key);
                }
            }

            // Validate the model using server-side validation
            if (!ModelState.IsValid)
            {
                // If invalid, gather errors and return a JSON response.
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                var errors2 = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                    );

                return Json(new { success = false, errors, errors2 });
            }

            var appraisal = await _appraisalService.AddAppraisalWFAsync(model);
            //return Json(new { success = true, data = dto });
            return Json(new { success = true, message = "Appraisal saved successfully", appId = appraisal.AppraisalId });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }
    #endregion Workflow-Action
    #endregion Workflow
    #region GRUD

    public async Task<IActionResult> Index()
    {
        var appraisals = await _appraisalService.GetAllAppraisalsAsync();
        return View(appraisals); // If using Razor View
    }

    public async Task<IActionResult> Details(int id)
    {
        var appraisal = await _appraisalService.GetAppraisalByIdAsync(id);
        if (appraisal == null) return NotFound();
        return View(appraisal);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(AppraisalDto dto)
    {
        if (!ModelState.IsValid) return View(dto);
        await _appraisalService.AddAppraisalAsync(dto);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var appraisal = await _appraisalService.GetAppraisalByIdAsync(id);
        return View(appraisal);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(AppraisalDto dto)
    {
        if (!ModelState.IsValid) return View(dto);
        await _appraisalService.UpdateAppraisalAsync(dto);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        await _appraisalService.DeleteAppraisalAsync(id);
        return RedirectToAction(nameof(Index));
    }
    #endregion GRUD
    #region Common
    private void SetGetUserInfo(EmployeeDto? employee, out string userName, out int? roleID, out int? userID, out int? managerID)
    {
        if (employee != null)
        {
            //Create Cookie
            Response.Cookies.Append("userID", Convert.ToString(employee.EmployeeId), new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddHours(1),
                HttpOnly = true
            });

            Response.Cookies.Append("userName", employee.EmployeeName, new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddHours(1),
                HttpOnly = true
            });

            Response.Cookies.Append("roleID", Convert.ToString(employee.RoleId), new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddHours(1),
                HttpOnly = true
            });

            Response.Cookies.Append("managerID", Convert.ToString(employee.ManagerId), new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddHours(1),
                HttpOnly = true
            });
            HttpContext.Session.SetString("userID", Convert.ToString(employee.EmployeeId));
            HttpContext.Session.SetString("userName", Convert.ToString(employee.EmployeeName));
            HttpContext.Session.SetString("roleID", Convert.ToString(employee.RoleId));
            HttpContext.Session.SetString("managerID", Convert.ToString(employee.ManagerId));
        }
        //Get Cookie
        string userNameVal = Request?.Cookies?["userName"] ?? Convert.ToString(HttpContext.Session.GetString("userName")) ?? "";
        string userIDVal = Request?.Cookies?["userID"] ?? Convert.ToString(HttpContext.Session.GetString("userID")) ?? "0";
        string roleIDVal = Request?.Cookies?["roleID"] ?? Convert.ToString(HttpContext.Session.GetString("roleID")) ?? "0";
        string managerIDVal = Request?.Cookies?["managerID"] ?? Convert.ToString(HttpContext.Session.GetString("managerID")) ?? "0";

        ViewBag.M_UserName = userNameVal;
        ViewBag.M_UserID = userIDVal;
        ViewBag.M_RoleID = roleIDVal;
        ViewBag.M_ManagerID = managerIDVal;

        userName = userNameVal;
        userID = Convert.ToInt32(userIDVal);
        roleID = Convert.ToInt32(roleIDVal);
        managerID = Convert.ToInt32(managerIDVal);
    }
    private void GetUserInfo(ref string userName, ref int? roleID, ref int? userID, ref int? managerID)
    {
        string userNameVal = Request?.Cookies?["userName"] ?? Convert.ToString(HttpContext.Session.GetString("userName")) ?? "";
        string userIDVal = Request?.Cookies?["userID"] ?? Convert.ToString(HttpContext.Session.GetString("userID")) ?? "0";
        string roleIDVal = Request?.Cookies?["roleID"] ?? Convert.ToString(HttpContext.Session.GetString("roleID")) ?? "0";
        string managerIDVal = Request?.Cookies?["managerID"] ?? Convert.ToString(HttpContext.Session.GetString("managerID")) ?? "0";

        ViewBag.M_UserName = userNameVal;
        ViewBag.M_UserID = userIDVal;
        ViewBag.M_RoleID = roleIDVal;
        ViewBag.M_ManagerID = managerIDVal;

        userName = userNameVal;
        userID = Convert.ToInt32(userIDVal);
        roleID = Convert.ToInt32(roleIDVal);
        managerID = Convert.ToInt32(managerIDVal);
        // if (string.IsNullOrWhiteSpace(userID) || userID == "0")
        // {
        //     Response.Redirect("~/Forms/Users/Logout");
        // }
    }
    private void GetUserData(ref string userName, ref int? roleID, ref int? userID)
    {
        if (Request != null && Request.Cookies != null && Request.Cookies["userName"] != null && !string.IsNullOrEmpty(Request.Cookies["userName"]))
            userName = Request.Cookies["userName"];
        if (Request != null && Request.Cookies != null && Request.Cookies["userID"] != null && !string.IsNullOrEmpty(Request.Cookies["userID"]))
            userID = Convert.ToInt32(Request.Cookies["userID"]);
        if (Request != null && Request.Cookies != null && Request.Cookies["roleID"] != null && !string.IsNullOrEmpty(Request.Cookies["roleID"]))
            roleID = Convert.ToInt32(Request.Cookies["roleID"]);

        if (string.IsNullOrWhiteSpace(userName) && (HttpContext.Session.GetString("userName") != null && !string.IsNullOrWhiteSpace(HttpContext.Session.GetString("userName").ToString())))
            userName = Convert.ToString(HttpContext.Session.GetString("userName"));
        if ((!userID.HasValue || userID == 0) && (HttpContext.Session.GetString("userID") != null && !string.IsNullOrWhiteSpace(HttpContext.Session.GetString("userID").ToString())))
            userID = Convert.ToInt32(HttpContext.Session.GetString("userID"));
        if ((!roleID.HasValue || roleID == 0) && (HttpContext.Session.GetString("roleID") != null && !string.IsNullOrWhiteSpace(HttpContext.Session.GetString("roleID").ToString())))
            roleID = Convert.ToInt32(HttpContext.Session.GetString("roleID"));
    }
    private void SetUserInViewBag1()
    {
        string userName = "";
        string userID = "0";
        string roleID = "0";
        string sectID = "0";
        string accessToken = "";

        if (Request != null && Request.Cookies != null && Request.Cookies["userName"] != null && !string.IsNullOrEmpty(Request.Cookies["userName"]))
            userName = Convert.ToString(Request?.Cookies?["userName"]);
        if (Request != null && Request.Cookies != null && Request.Cookies["userID"] != null && !string.IsNullOrEmpty(Request.Cookies["userID"]))
            userID = Convert.ToString(Request.Cookies["userID"]);
        if (Request != null && Request.Cookies != null && Request.Cookies["roleID"] != null && !string.IsNullOrEmpty(Request.Cookies["roleID"]))
            roleID = Convert.ToString(Request.Cookies["roleID"]);
        if (Request != null && Request.Cookies != null && Request.Cookies["sectID"] != null && !string.IsNullOrEmpty(Request.Cookies["sectID"]))
            sectID = Convert.ToString(Request.Cookies["sectID"]);
        if (Request != null && Request.Cookies != null && Request.Cookies["accessToken"] != null && !string.IsNullOrEmpty(Request.Cookies["accessToken"]))
            accessToken = Convert.ToString(Request.Cookies["accessToken"]);


        if (string.IsNullOrWhiteSpace(userName))
        {
            if (HttpContext.Session.GetString("userName") != null && !string.IsNullOrWhiteSpace(HttpContext.Session.GetString("userName").ToString()))
            { ViewBag.M_UserName = Convert.ToString(HttpContext.Session.GetString("userName")); userName = Convert.ToString(HttpContext.Session.GetString("userName")); }
            else { ViewBag.M_UserName = ""; }
        }
        else { ViewBag.M_UserName = userName; }

        if (userID == "0")
        {
            if (HttpContext.Session.GetString("userID") != null && !string.IsNullOrWhiteSpace(HttpContext.Session.GetString("userID").ToString()))
            { ViewBag.M_UserID = Convert.ToString(HttpContext.Session.GetString("userID")); userID = Convert.ToString(HttpContext.Session.GetString("userID")); }
            else { ViewBag.M_UserID = "0"; }
        }
        else { ViewBag.M_UserID = userID; }

        if (roleID == "0")
        {
            if (HttpContext.Session.GetString("roleID") != null && !string.IsNullOrWhiteSpace(HttpContext.Session.GetString("roleID").ToString()))
            { ViewBag.M_RoleID = Convert.ToString(HttpContext.Session.GetString("roleID")); roleID = Convert.ToString(HttpContext.Session.GetString("roleID")); }
            else { ViewBag.M_RoleID = "0"; }
        }
        else { ViewBag.M_RoleID = roleID; }

        if (sectID == "0")
        {
            if (HttpContext.Session.GetString("sectID") != null && !string.IsNullOrWhiteSpace(HttpContext.Session.GetString("sectID").ToString()))
            { ViewBag.M_SectID = Convert.ToString(HttpContext.Session.GetString("sectID")); sectID = Convert.ToString(HttpContext.Session.GetString("sectID")); }
            else { ViewBag.M_SectID = "0"; }
        }
        else { ViewBag.M_SectID = sectID; }


        if (userID == "0")
            Response.Redirect("~/Forms/Users/Logout");
    }
    private async void SetUserInfo(string userId)
    {

        if (!string.IsNullOrWhiteSpace(userId))
        {
            var employee = await _employeeService.GetByIdAsync(Convert.ToInt32(userId));
            if (employee != null)
            {
                //Create Cookie
                Response.Cookies.Append("userID", Convert.ToString(employee.EmployeeId), new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddHours(1),
                    HttpOnly = true
                });

                Response.Cookies.Append("userName", employee.EmployeeName, new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddHours(1),
                    HttpOnly = true
                });

                Response.Cookies.Append("roleID", Convert.ToString(employee.RoleId), new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddHours(1),
                    HttpOnly = true
                });

                Response.Cookies.Append("managerID", Convert.ToString(employee.ManagerId), new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddHours(1),
                    HttpOnly = true
                });
                // HttpCookie aCookie = new HttpCookie("userInfo");
                // if (aCookie == null)
                // {
                //     return HttpNotFound();
                // }
                // aCookie.Values["userID"] = Convert.ToString(employee.EmployeeId);
                // aCookie.Values["userName"] = Convert.ToString(employee.EmployeeName);
                // aCookie.Values["roleID"] = Convert.ToString(employee.RoleId);
                // aCookie.Values["managerID"] = Convert.ToString(employee.ManagerId);
                HttpContext.Session.SetString("userID", Convert.ToString(employee.EmployeeId));
                HttpContext.Session.SetString("userName", Convert.ToString(employee.EmployeeName));
                HttpContext.Session.SetString("roleID", Convert.ToString(employee.RoleId));
                HttpContext.Session.SetString("managerID", Convert.ToString(employee.ManagerId));
            }
        }
    }


    #endregion Common
}
