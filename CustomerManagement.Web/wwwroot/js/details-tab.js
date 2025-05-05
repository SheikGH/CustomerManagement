var site = site || {};
site.baseUrl = site.baseUrl || "";

$(document).ready(function () {
    // locate each partial section.
    // if it has a URL set, load the contents into the area.
    
    // DEMO ONLY - JUST IGNORE
    // Just to make the loading time obvious....
    $("a.nav").click(function () {
        $("body").html("");
    });
    var tabSiteUrl = null;
    if (jsGlobalTabSiteUrl !== undefined && jsGlobalTabSiteUrl !== NaN && jsGlobalTabSiteUrl !== null && jsGlobalTabSiteUrl !== "")
        tabSiteUrl = jsGlobalTabSiteUrl;

    if (tabSiteUrl === undefined || tabSiteUrl === null || tabSiteUrl === NaN || tabSiteUrl === "")
        tabSiteUrl = window.location.href;
    //InitTab("");
    //$(document).ajaxSend(function () {
    //    $("#overlay").fadeIn(300);
    //}).ajaxStop(function () {
    //    setTimeout(function () {
    //        $("#overlay").fadeOut(300);
    //    }, 500);
    //});
    $('#tabstrip a').click(function (e) {
        e.preventDefault();
        //$(document).ajaxSend(function () {
        //    $("#overlay").fadeIn(300);
        //});
        var tabID = $(this).attr("href").substr(1);
        if (tabID !== "tabDetail" && tabID !== "tabFormEdit" && tabSiteUrl.search('DetailsTab') != -1) {
            //$(".tab-pane").each(function () {
            //    if ($(this).attr("id") !== "tabFormEdit" && $(this).attr("id") !== "tabDetail" && tabSiteUrl.search('DetailsTab') != -1) {
            //        if ($(this).attr("id") === tabID) {
            //            console.log("clearing " + $(this).attr("id") + " tab");
            //            $(this).empty();
            //        }
            //    }
            //});
            //InitTab(tabID);
            //debugger;
            if (tabSiteUrl.indexOf("tid=") == -1) { return }
            if (tabID == "") { return }
            $.ajax({
                beforeSend: function () {
                    // Handle the beforeSend event
                    $("#overlay").fadeIn(300);
                },
                complete: function () {
                    // Handle the complete event
                    setTimeout(function () {
                        $("#overlay").fadeOut(300);
                    }, 500);
                },
                url: tabSiteUrl + "&t=" + tabID,
                cache: false,
                type: "get",
                dataType: "html",
                success: function (result) {
                    //console.log("Result::", result);
                    $("#" + tabID).empty();
                    //$("#" + tabID).html("");
                    $("#" + tabID).html(result);
                }
            });
            //    .done(function () {
            //    setTimeout(function () {
            //        $("#overlay").fadeOut(300);
            //    }, 500);
            //});
            //$(this).tab('show');
        }
    });

});
