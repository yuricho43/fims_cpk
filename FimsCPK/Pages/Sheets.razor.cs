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

namespace FimsCPK.Pages
{
    public partial class Sheets
    {
        public List<Tsheet> gSheetList { get; set; }

        private DateTime gStartYear = DateTime.Now;
        private DateTime gEndYear = DateTime.Now;
        private string gStringUser = "FSTAdmin";
        private string gStringCompany = "";
        private string gStringManageSite = "";
        private string gStringCountry = "";
        private string gStringCustomer = "";
        private bool gSetDuration = false;
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
            gStartYear = gEndYear.AddMonths(-12);
        }

        private void OnStateInit(GridStateEventArgs<Tsheet> args)
        {
            SearchRecord();
        }

        async Task SearchRecord()
        {
            string strStartDate = gStartYear.ToString("yyyy-MM-dd");
            string strEndDate = gEndYear.ToString("yyyy-MM-dd");

            using (var db = new FimsDbContext())
            {
                if (gSetDuration == false) // 날짜 무시
                    gSheetList = db.Tsheets.ToList();
                else
                    gSheetList = db.Tsheets.OrderBy(x=>x.InspectionEndDateTime).Where(n => n.InspectionEndDateTime >= gStartYear && n.InspectionEndDateTime <= gEndYear).ToList();
                // gSheetList = db.Tsheets.Where(n => n.CreatedOn.ToString("yyyy-MM-dd").Length >= 10 && String.Compare(n.CreatedOn.ToString("yyyy-MM-dd"), strStartDate) >= 0 && String.Compare(n.CreatedOn.ToString("yyyy-MM-dd"), strEndDate) <= 0).ToList();
                gCounter = gSheetList.Count;
            }
        }
    }
}