using CustomerManagement.Core.Interfaces;
using CustomerManagement.Core.Entities;
using CustomerManagement.Infrastructure.Persistence.Data;
using CustomerManagement.Common.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;
using Dapper;

namespace CustomerManagement.Infrastructure.Persistence.Repositories
{
    public class AppraisalRepository : IAppraisalRepository
    {
        private readonly AppDbContext _context;

        public AppraisalRepository(AppDbContext context)
        {
            _context = context;
        }

        #region Workflow
        public async Task<IEnumerable<AppraisalTaskDto>> GetByUIdAsync(SearchInParamDto param)
        {
            var appraisalTaskDtoList = new List<AppraisalTaskDto>();
            try
            {
                using (var connection = _context.Database.GetDbConnection())
                {
                    await connection.OpenAsync();
                    var parameters = new DynamicParameters();
                    parameters.Add("@DBAction", param.DBAction);
                    parameters.Add("@MenuAction", param.MenuAction);
                    parameters.Add("@AppraisalId", param.AppraisalId);
                    parameters.Add("@LoginId", param.LoginId);
                    parameters.Add("@RoleId", param.RoleId);
                    parameters.Add("@SortColumn", param.SortColumn);
                    parameters.Add("@SortDir", param.SortDir);
                    parameters.Add("@SearchVal", param.SearchVal);
                    parameters.Add("@Skip", param.Skip ?? 0);
                    parameters.Add("@PageSize", param.PageSize ?? 10);
                    parameters.Add("@UrlDetail", param.UrlDetail);
                    parameters.Add("@UrlDelete", param.UrlDelete);

                    var result = await connection.QueryAsync<AppraisalTaskDto>(
                        "sp_GetAppraisalBySearch",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );
                    return result.ToList();
                }
                
            }
            catch (Exception ex)
            {
                return appraisalTaskDtoList;
                //throw;
            }

        }
        public async Task<AppraisalDetailDto> GetDetailsAsync(SearchInParamDto param)
        {
            var appraisalDetail = new AppraisalDetailDto();
            try
            {
                using (var connection = _context.Database.GetDbConnection())
                {
                    await connection.OpenAsync();
                    var parameters = new DynamicParameters();
                    parameters.Add("@DBAction", param.DBAction);
                    parameters.Add("@MenuAction", param.MenuAction);
                    parameters.Add("@AppraisalId", param.AppraisalId);
                    parameters.Add("@LoginId", param.LoginId);
                    parameters.Add("@RoleId", param.RoleId);
                    parameters.Add("@SortColumn", param.SortColumn);
                    parameters.Add("@SortDir", param.SortDir);
                    parameters.Add("@SearchVal", param.SearchVal);
                    parameters.Add("@Skip", param.Skip ?? 0);
                    parameters.Add("@PageSize", param.PageSize ?? 10);
                    parameters.Add("@UrlDetail", param.UrlDetail);
                    parameters.Add("@UrlDelete", param.UrlDelete);

                    using var multi = await connection.QueryMultipleAsync(
                        "sp_GetAppraisalBySearch",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );
                    appraisalDetail.AppraisalDto = await multi.ReadFirstOrDefaultAsync<AppraisalDto>();
                    appraisalDetail.AppraisalTaskDto = await multi.ReadFirstOrDefaultAsync<AppraisalTaskDto>();
                    appraisalDetail.AppraisalTaskHistoryDtos = (await multi.ReadAsync<AppraisalTaskHistoryDto>()).ToList();
                }

            }
            catch (Exception ex)
            {

                //throw;
            }
            return appraisalDetail;
        }
        public async Task<AppraisalTaskDto?> GetWFBySearchAsync(SearchInParamDto param)
        {
            try
            {
                var result = await _context.AppraisalTaskDtos
                    .FromSqlRaw(
                        "EXEC sp_GetAppraisalBySearch " +
                        "@DBAction = @DBActionParam, @MenuAction = @MenuActionParam, @AppraisalId = @AppraisalIdParam, " +
                        "@LoginId = @LoginIdParam, @RoleId = @RoleIdParam, @SortColumn = @SortColumnParam, @SortDir = @SortDirParam, " +
                        "@SearchVal = @SearchValParam, @Skip = @SkipParam, @PageSize = @PageSizeParam, " +
                        "@UrlDetail = @UrlDetailParam, @UrlDelete = @UrlDeleteParam",
                        new SqlParameter("@DBActionParam", param.DBAction ?? (object)DBNull.Value),
                        new SqlParameter("@MenuActionParam", param.MenuAction ?? (object)DBNull.Value),
                        new SqlParameter("@AppraisalIdParam", param.AppraisalId ?? (object)DBNull.Value),
                        new SqlParameter("@LoginIdParam", param.LoginId ?? (object)DBNull.Value),
                        new SqlParameter("@RoleIdParam", param.RoleId ?? (object)DBNull.Value),
                        new SqlParameter("@SortColumnParam", param.SortColumn ?? (object)DBNull.Value),
                        new SqlParameter("@SortDirParam", param.SortDir ?? (object)DBNull.Value),
                        new SqlParameter("@SearchValParam", param.SearchVal ?? (object)DBNull.Value),
                        new SqlParameter("@SkipParam", param.Skip ?? 0),
                        new SqlParameter("@PageSizeParam", param.PageSize ?? 10),
                        new SqlParameter("@UrlDetailParam", param.UrlDetail ?? (object)DBNull.Value),
                        new SqlParameter("@UrlDeleteParam", param.UrlDelete ?? (object)DBNull.Value)
                    )
                    .ToListAsync();

                return result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                // You can log the exception here
                return null;
            }
        }

        public async Task<AppraisalDto> AddWFAsync(Appraisal appraisal, string dbAction)
        {
            try
            {
                using (var connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
                {
                    await connection.OpenAsync(); // Important to open the connection before executing

                    var parameters = new DynamicParameters();

                    parameters.Add("@DBAction", dbAction ?? string.Empty, DbType.String);

                    parameters.Add("@AppraisalId", appraisal.AppraisalId, DbType.Int32);
                    parameters.Add("@RoleId", appraisal.RoleId, DbType.Int32);

                    parameters.Add("@AssignDate", appraisal.AssignDate ?? (object)DBNull.Value, DbType.DateTime);
                    parameters.Add("@AssignBy", appraisal.AssignBy ?? (object)DBNull.Value, DbType.String);
                    parameters.Add("@AssignTo", appraisal.AssignTo ?? (object)DBNull.Value, DbType.String);

                    parameters.Add("@ActionId", appraisal.ActionId ?? (object)DBNull.Value, DbType.Int32);
                    parameters.Add("@ActionDate", appraisal.ActionDate ?? (object)DBNull.Value, DbType.DateTime);

                    parameters.Add("@Title", appraisal.Title ?? (object)DBNull.Value, DbType.String);
                    parameters.Add("@Message", appraisal.Message ?? (object)DBNull.Value, DbType.String);

                    parameters.Add("@Weight", appraisal.Weight.HasValue ? Math.Round(appraisal.Weight.Value, 2) : (object)DBNull.Value, DbType.Decimal);
                    parameters.Add("@Achievement", appraisal.Achievement.HasValue ? Math.Round(appraisal.Achievement.Value, 2) : (object)DBNull.Value, DbType.Decimal);
                    parameters.Add("@EmployeeScore", appraisal.EmployeeScore.HasValue ? Math.Round(appraisal.EmployeeScore.Value, 2) : (object)DBNull.Value, DbType.Decimal);
                    parameters.Add("@ManagerScore", appraisal.ManagerScore.HasValue ? Math.Round(appraisal.ManagerScore.Value, 2) : (object)DBNull.Value, DbType.Decimal);

                    parameters.Add("@EmployeeComment", appraisal.EmployeeComment ?? (object)DBNull.Value, DbType.String);
                    parameters.Add("@ManagerComment", appraisal.ManagerComment ?? (object)DBNull.Value, DbType.String);
                    parameters.Add("@CurrentStatus", appraisal.CurrentStatus ?? (object)DBNull.Value, DbType.String);
                    var result = await connection.QueryFirstOrDefaultAsync<AppraisalDto>(
                                    "sp_AppraisalWorkflow",
                                    parameters,
                                    commandType: CommandType.StoredProcedure);
                    return result;
                    //await connection.ExecuteAsync("sp_AppraisalWorkflow", parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                return null;
                // Consider logging
                //throw new Exception("Error executing stored procedure: sp_AppraisalWorkflow", ex);

            }
        }



        #endregion Workflow

        #region CRUD
        public async Task<IEnumerable<Appraisal>> GetAllAsync()
        {
            return await _context.Appraisals.ToListAsync();
        }

        public async Task<Appraisal> GetByIdAsync(int id)
        {
            return await _context.Appraisals.FindAsync(id);
        }

        public async Task AddAsync(Appraisal appraisal)
        {
            _context.Appraisals.Add(appraisal);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Appraisal appraisal)
        {
            _context.Appraisals.Update(appraisal);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var appraisal = await _context.Appraisals.FindAsync(id);
            if (appraisal != null)
            {
                _context.Appraisals.Remove(appraisal);
                await _context.SaveChangesAsync();
            }
        }
        #endregion CRUD
    }

}