using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.JSInterop;
using FimsCPK;
using FimsCPK.Shared;
using Telerik.Blazor;
using Telerik.Blazor.Components;
using FimsCPK.Models;
using Telerik.DataSource;
using Telerik.DataSource.Extensions;
using Telerik.FontIcons;
using static FimsCPK.Pages.Cpk;
using System.Diagnostics.Eventing.Reader;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FimsCPK.Pages
{
    public partial class Sheets
    {
        static List<string> gModelNames = new List<string>();
        public List<Tsheet> gSheetList { get; set; }

        private DateTime gStartYear = DateTime.Now;
        private DateTime gEndYear = DateTime.Now;
        private string gStringUser = "FSTAdmin";
        private string gStringModel = "";
        private string gStringInspector = "";
        private string gStringCloser = "";
        private bool gSetDuration = true;
        private int gCounter = 0;
        public DateTime gMinYear = new DateTime(2023, 1, 1, 0, 0, 0);
        public DateTime gMaxYear = new DateTime(2030, 1, 1, 0, 0, 0);
        public int gSpanYear = 0;
        public int gPageSize = 100;
        public List<int?> gPageList = new List<int?>
        {
            10,
            15,
            20,
            30,
            50,
            100,
            500,
            null
        };
        protected override async Task OnInitializedAsync()
        {
            gStartYear = gEndYear.AddMonths(-3);
            SearchRecord();
        }

        private void OnStateInit(GridStateEventArgs<Tsheet> args)
        {
            //SearchRecord();
            List_Models();
        }

        private void List_Models()
        {
            //--- if alread models are listed-up, skip
            if (gModelNames.Count > 0)
                return;

            using (var db = new FimsDbContext())
            {
                gModelNames = (from sheet in db.Tsheets
                         orderby sheet.ProductModel
                         group sheet by sheet.ProductModel into grp
                         select grp.Key).ToList();
            }
        }


        // 날짜, Model, (검사자, 마감자) 
        async Task SearchRecord()
        {
            string strStartDate = gStartYear.ToString("yyyy-MM-dd");
            string strEndDate = gEndYear.ToString("yyyy-MM-dd");
            byte bCondition = 0;    // bit3:날짜, bit2:모델, bit1:검사자, bit
            using (var db = new FimsDbContext())
            {
                if (gSetDuration == false)
                { 
                    if (gStringModel != null && gStringModel.Length >=2)
                        gSheetList = db.Tsheets.OrderByDescending(x => x.InspectionEndDateTime).
                                    Where(n => n.ProductModel.Contains(gStringModel) == true
                                    ).ToList();
                    else
                        gSheetList = db.Tsheets.OrderByDescending(x => x.InspectionEndDateTime).ToList();
                }
                else
                {
                    if (gStringModel != null && gStringModel.Length >= 2)
                       gSheetList = db.Tsheets.OrderByDescending(x => x.InspectionEndDateTime).
                            Where(n => n.InspectionEndDateTime >= gStartYear && n.InspectionEndDateTime <= gEndYear &&
                                  n.ProductModel.Contains(gStringModel) == true
                                  ).ToList();
                    else
                        gSheetList = db.Tsheets.OrderByDescending(x => x.InspectionEndDateTime).Where(n => n.InspectionEndDateTime >= gStartYear && n.InspectionEndDateTime <= gEndYear).ToList();
                }
                gCounter = gSheetList.Count;
            }
        }
    }
}