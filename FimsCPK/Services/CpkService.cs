using DocumentFormat.OpenXml.InkML;
using FimsCPK.Data;
using FimsCPK.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Telerik.SvgIcons;
using static FimsCPK.Pages.Home;
using static Telerik.Blazor.ThemeConstants;

namespace FimsCPK.Services
{
    public class CpkService
    {
        private readonly FimsDbContext _dbFimsContext;
        static List<string> gStrModels = new List<string>();

        public class ModelCount
        {
            public string model { get; set; }
            public int count { get; set; }
        }

        /// <summary>
        /// Reserve DbContext
        /// </summary>
        /// <param name="dbContext"></param>
        public CpkService(FimsDbContext dbContext)
        {
            _dbFimsContext = dbContext;
        }

        /// <summary>
        /// Get All CpkItems
        /// </summary>
        /// <returns></returns>
        public List<CpkItem> GetCpkItems()
        {
            var CpkItemLists = new List<CpkItem>();

            CpkItemLists = _dbFimsContext.CpkItems.ToList();
            return CpkItemLists;
        }

        /// <summary>
        /// Get a specific CpkItem
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CpkItem GetCpkItemById(int id)
        {
            int cpkId = id;

            var cpk = _dbFimsContext.CpkItems.FirstOrDefault(p => p.Id == id);

            if (cpk is null)
                return null;

            return new CpkItem()
            {
                Id = cpk.Id,
                TestNo = cpk.TestNo,
                Ch1Lcl = cpk.Ch1Lcl,
                Ch1Ucl = cpk.Ch1Ucl,
                Ch2Lcl = cpk.Ch2Lcl,
                Ch2Ucl = cpk.Ch2Ucl,
                Ch3Lcl = cpk.Ch3Lcl,
                Ch3Ucl = cpk.Ch3Ucl,
                Ch4Lcl = cpk.Ch4Lcl,
                Ch4Ucl = cpk.Ch4Ucl,
            };
        }

        public string CreateCpkItem(CpkItem cpkDto)
        {
            var newcpk = new CpkItem()
            {
                TestNo = cpkDto.TestNo,
                Model = cpkDto.Model,
                Ch1Lcl = cpkDto.Ch1Lcl,
                Ch1Ucl = cpkDto.Ch1Ucl,
                Ch2Lcl = cpkDto.Ch2Lcl,
                Ch2Ucl = cpkDto.Ch2Ucl,
                Ch3Lcl = cpkDto.Ch3Lcl,
                Ch3Ucl = cpkDto.Ch3Ucl,
                Ch4Lcl = cpkDto.Ch4Lcl,
                Ch4Ucl = cpkDto.Ch4Ucl,
            };

            _dbFimsContext.CpkItems.Add(newcpk);
            _dbFimsContext.SaveChanges();
            return "Create successfully";
        }
        public string CreateCpkItemFromForm(NewCpkRegisterRequestModel cpkRequest)
        {
            var newcpk = new CpkItem()
            {
                TestNo = cpkRequest.iTestNo,
                Model = cpkRequest.ModelName,
                Ch1Lcl = cpkRequest.LCL1,
                Ch1Ucl = cpkRequest.UCL1,
                Ch2Lcl = cpkRequest.LCL2,
                Ch2Ucl = cpkRequest.UCL2,
                Ch3Lcl = cpkRequest.LCL3,
                Ch3Ucl = cpkRequest.UCL3,
                Ch4Lcl = cpkRequest.LCL4,
                Ch4Ucl = cpkRequest.UCL4,
            };

            _dbFimsContext.CpkItems.Add(newcpk);
            _dbFimsContext.SaveChanges();
            return "Create successfully";
        }

        public string UpdateCpkItem(CpkItem cpkDto)
        {
            //--- get CpkItem
            var cpkItem = _dbFimsContext.CpkItems.FirstOrDefault(p => p.Id == cpkDto.Id);

            if (cpkItem is null)
                return "There is no such a CpkItem";

            try
            {
                //--- Set to new value
                CpkItem cpkNew = new CpkItem();
                cpkNew = cpkItem;

                cpkNew.Ch1Lcl = cpkDto.Ch1Lcl;
                cpkNew.Ch1Ucl = cpkDto.Ch1Ucl;
                cpkNew.Ch2Lcl = cpkDto.Ch2Lcl;
                cpkNew.Ch2Ucl = cpkDto.Ch2Ucl;
                cpkNew.Ch3Lcl = cpkDto.Ch3Lcl;
                cpkNew.Ch3Ucl = cpkDto.Ch3Ucl;
                cpkNew.Ch4Lcl = cpkDto.Ch4Lcl;
                cpkNew.Ch4Ucl = cpkDto.Ch4Ucl;

                //--- Update
                _dbFimsContext.Entry(cpkItem).CurrentValues.SetValues(cpkNew);
                _dbFimsContext.SaveChanges();

                return "Updated successfully";
            }
            catch (Exception ex)
            {
                throw;
            }
            return "Update failed";
        }

        public string UpdateInspectionItem(Titem itemUpdate)
        {
            //--- get CpkItem
            var itemDb = _dbFimsContext.Titems.FirstOrDefault(p => p.Id == itemUpdate.Id);

            if (itemDb is null)
                return "There is no such a TItem";

            try
            {
                //--- Set to new value
                Titem itemNew = new Titem();
                itemNew = itemDb;

                itemNew.Ch1Data = itemUpdate.Ch1Data;
                itemNew.Ch2Data = itemUpdate.Ch2Data;
                itemNew.Ch3Data = itemUpdate.Ch3Data;
                itemNew.ModifiedOn = DateTime.Now;
                //--- Update
                _dbFimsContext.Entry(itemDb).CurrentValues.SetValues(itemNew);
                _dbFimsContext.SaveChanges();

                return "Updated successfully";
            }
            catch (Exception ex)
            {
                throw;
            }
            return "Update failed";
        }


        public string DeleteCpkItem(string Model, int TestNo)
        {
            //=== Delete POManage
            var cpk = _dbFimsContext.CpkItems.FirstOrDefault(p => p.Model == Model && p.TestNo == TestNo);
            if (cpk is null)
                return "Cpk not found";

            _dbFimsContext.CpkItems.Remove(cpk);
            _dbFimsContext.SaveChanges();

            return "Delete successfully";
        }

        public List<TspecItem> GetSLValuesForModelAndTestNo(string strModel, List<int> listTestNo)
        {
            int idModel = _dbFimsContext.TspecModels.Where(x => x.ProductModel == strModel).Select(p => p.Id).FirstOrDefault();
            if (idModel <= 0)
                idModel = _dbFimsContext.TspecModels.Where(x => x.ProductModel.Contains(strModel) == true).Select(p => p.Id).FirstOrDefault();

            List<TspecItem> tCpkItemsSL = _dbFimsContext.TspecItems.Where(x => idModel == x.TspecModelId && listTestNo.Contains(x.TestNo)).ToList();
            foreach (var item in tCpkItemsSL)
            {
                item.Ch4Ucl = _dbFimsContext.TspecItems.Where(x => x.TestNo == item.TestNo).Select(p => p.Title).FirstOrDefault();
            }
            return tCpkItemsSL;
        }

        public TspecItem GetOneSLValueForModelAndTestNo(string strModel, int TestNo)
        {
            int idModel = _dbFimsContext.TspecModels.Where(x => x.ProductModel == strModel).Select(p => p.Id).FirstOrDefault();
            TspecItem tCpkItemSL = _dbFimsContext.TspecItems.Where(x => idModel == x.TspecModelId && x.TestNo == TestNo).FirstOrDefault();
            return tCpkItemSL;
        }

        public List<CpkItem> GetCLValuesForModel(string strModel)
        {
            //--- remove  (   ). ex CD222(NXP) ==> CD222
            /*
            int ix = strModel.IndexOf("(");
            if (ix > 0)
            {
                strModel = strModel.Substring(0, ix);
            }
            */

            List<CpkItem> tCpkItemsCL = _dbFimsContext.CpkItems.Where(x => x.Model == strModel).OrderBy(x => x.TestNo).ToList();
            foreach (var item in tCpkItemsCL)
            {
                item.Reserved1 = _dbFimsContext.TspecItems.Where(x => x.TestNo == item.TestNo).Select(p => p.Title).FirstOrDefault();
            }
            return tCpkItemsCL;
        }

        //--- 특정모델의 TestNo에 대해, ixCh의 SL값을 return
        public void GetSL(string strModel, int TestNo, int ixCh, ref string strLSL, ref string strUSL)
        {
            int idModel = _dbFimsContext.TspecModels.Where(x => x.ProductModel == strModel).Select(p => p.Id).FirstOrDefault();
            if (ixCh == 1)
            {
                strLSL = _dbFimsContext.TspecItems.Where(x => idModel == x.TspecModelId && TestNo == x.TestNo).Select(p => p.Ch1Lcl).FirstOrDefault();
                strUSL = _dbFimsContext.TspecItems.Where(x => idModel == x.TspecModelId && TestNo == x.TestNo).Select(p => p.Ch1Ucl).FirstOrDefault(); ;
            }
            else if (ixCh == 2)
            {
                strLSL = _dbFimsContext.TspecItems.Where(x => idModel == x.TspecModelId && TestNo == x.TestNo).Select(p => p.Ch2Lcl).FirstOrDefault();
                strUSL = _dbFimsContext.TspecItems.Where(x => idModel == x.TspecModelId && TestNo == x.TestNo).Select(p => p.Ch2Ucl).FirstOrDefault(); ;
            }
            else if (ixCh == 3)
            {
                strLSL = _dbFimsContext.TspecItems.Where(x => idModel == x.TspecModelId && TestNo == x.TestNo).Select(p => p.Ch3Lcl).FirstOrDefault();
                strUSL = _dbFimsContext.TspecItems.Where(x => idModel == x.TspecModelId && TestNo == x.TestNo).Select(p => p.Ch3Ucl).FirstOrDefault(); ;
            }
            else
            {
                strLSL = "";
                strUSL = "";
            }
        }

        //--- 특정모델의 TestNo에 대해, ixCh의 SL값을 return
        public void GetCL(string strModel, int TestNo, int ixCh, ref string strLCL, ref string strUCL)
        {
            if (ixCh == 1)
            {
                strLCL = _dbFimsContext.CpkItems.Where(x => x.Model == strModel && TestNo == x.TestNo).Select(p => p.Ch1Lcl).FirstOrDefault();
                strUCL = _dbFimsContext.CpkItems.Where(x => x.Model == strModel && TestNo == x.TestNo).Select(p => p.Ch1Ucl).FirstOrDefault();
            }
            else if (ixCh == 2)
            {
                strLCL = _dbFimsContext.CpkItems.Where(x => x.Model == strModel && TestNo == x.TestNo).Select(p => p.Ch2Lcl).FirstOrDefault();
                strUCL = _dbFimsContext.CpkItems.Where(x => x.Model == strModel && TestNo == x.TestNo).Select(p => p.Ch2Ucl).FirstOrDefault();
            }
            else if (ixCh == 3)
            {
                strLCL = _dbFimsContext.CpkItems.Where(x => x.Model == strModel && TestNo == x.TestNo).Select(p => p.Ch3Lcl).FirstOrDefault();
                strUCL = _dbFimsContext.CpkItems.Where(x => x.Model == strModel && TestNo == x.TestNo).Select(p => p.Ch3Ucl).FirstOrDefault();
            }
            else
            {
                strLCL = "";
                strUCL = "";
            }
        }

        //--- Get Model List for Tested Models
        /// <summary>
        /// 
        /// </summary>
        /// <param name="iLowLimit">최소 검사 장비 수</param>
        /// <returns></returns>
        public List<string> GetCpkModelNames(int iMinNumEquips)
        {
            gStrModels.Clear();
            //--- if already extract target models
            if (gStrModels.Count > 0)
                return gStrModels;

            List<Tsheet> tCpkModelList;

            using (var db = new FimsDbContext())
            {
                tCpkModelList = db.Tsheets.ToList();
            }

            List<ModelCount> models = (from cpkModel in tCpkModelList
                                       orderby cpkModel.ProductModel
                                       group cpkModel by cpkModel.ProductModel into grp
                                       select new ModelCount() { model = grp.Key, count = grp.Count() }).ToList();

            //models = models.OrderByDescending(n=>n.count).ToList();

            foreach (var item in models)
            {
                if (item.count >= iMinNumEquips)
                    gStrModels.Add(item.model.ToString());
            }

            return gStrModels;
        }

        //--- Get Model List for Tested Models
        public List<string> GetCpkModelNamesForSetting()
        {
            List<string> strModels = new List<string>();
            List<CpkItem> tCpkModelList;

            using (var db = new FimsDbContext())
            {
                tCpkModelList = db.CpkItems.ToList();
            }

            List<ModelCount> models = (from cpkModel in tCpkModelList
                                       orderby cpkModel.Model
                                       group cpkModel by cpkModel.Model into grp
                                       select new ModelCount() { model = grp.Key, count = grp.Count() }).ToList();

            //models = models.OrderByDescending(n=>n.count).ToList();

            foreach (var item in models)
            {
                strModels.Add(item.model.ToString());
            }

            return strModels;
        }


        //=================================================
        // Utility Functions
        public string GetTestNameFromTestNo(int iTestNo)
        {
            string TestName = _dbFimsContext.TspecItems.Where(x => x.TestNo == iTestNo).Select(p => p.Title).FirstOrDefault();

            return TestName;
        }

        //--- CpkItem을 DB에 추가
        //    1) 특정 모델에 item을 DB에 저장하기
        //       - Model에 해당 TestNo가 존재 하는지 체크  ==> -1 return
        //       - DB에 저장
        //    2) 모든 모델리스트에 대해 1)을 반복
        //       - 
        public async Task<int> AddCpkItemAsync(NewCpkRegisterRequestModel newItem)
        {
            if (newItem.bForAllModel == false)
            {
                var result = _dbFimsContext.CpkItems.Where(n => n.TestNo == newItem.iTestNo && n.Model == newItem.ModelName).FirstOrDefault();
                if (result != null)
                    return -1;      // already exist

                CreateCpkItemFromForm(newItem);
            }
            else
            {
                List<string> listModels = GetCpkModelNamesForSetting();

                foreach (string modelname in listModels)
                {
                    newItem.ModelName = modelname;
                    var result = _dbFimsContext.CpkItems.Where(n => n.TestNo == newItem.iTestNo && n.Model == newItem.ModelName).FirstOrDefault();
                    if (result != null)
                        continue;

                    CreateCpkItemFromForm(newItem);
                }
            }
            return 1;
        }


        /// <summary>
        /// 1) Get Id for selected Model
        //  2) Delete TSpecModel
        //  3) Delete TSpecItems
        /// </summary>
        /// <param name="strModel"></param>
        /// <returns></returns>
        public async Task<int> DeleteTSpecModel(string strModel)
        {
            int IdModel = _dbFimsContext.TspecModels.Where(x => x.ProductModel == strModel).Select(y => y.Id).FirstOrDefault();
            if (IdModel == 0)
                return -1;  // nothing found

            var modelToDelete = _dbFimsContext.TspecModels.Where(x => x.ProductModel == strModel).ToList();
            _dbFimsContext.TspecModels.RemoveRange(modelToDelete);
            _dbFimsContext.SaveChanges();

            var itemToDelete = _dbFimsContext.TspecItems.Where(x => x.TspecModelId == IdModel).ToList();
            _dbFimsContext.TspecItems.RemoveRange(itemToDelete);
            _dbFimsContext.SaveChanges();

            return 1;
        }

        public async Task DeleteAllDataTSpecModel_Item()
        {
            var modelToDelete = _dbFimsContext.TspecModels.ToList();
            _dbFimsContext.TspecModels.RemoveRange(modelToDelete);
            _dbFimsContext.SaveChanges();

            var itemToDelete = _dbFimsContext.TspecItems.ToList();
            _dbFimsContext.TspecItems.RemoveRange(itemToDelete);
            _dbFimsContext.SaveChanges();
        }
        /// <summary>
        ///  1) Model 이름이 존재하는지 체크
        ///  2) TSpecModel 추가
        ///  3) TSpecItem 추가
        /// </summary>
        /// <param name="strModel"></param>
        /// <returns></returns>
        public async Task<int> AddTSpecModel(string strModel, List<TspecItem> items)
        {
            int IdModel = _dbFimsContext.TspecModels.Where(x => x.ProductModel == strModel).Select(y => y.Id).FirstOrDefault();
            if (IdModel > 0)
                return -1;  //already Exist

            //--- Add SpecModel
            var nsm = new TspecModel()
            {
                ProductModel = strModel,
                NumChannel = 10,
                CreatorName = "조용섭",
                CreatedOn = DateTime.Now,
            };

            _dbFimsContext.TspecModels.Add(nsm);
            _dbFimsContext.SaveChanges();

            //--- Add Spec Item
            int newId = nsm.Id;
            foreach (var item in items)
            {
                item.TspecModelId = newId;
                _dbFimsContext.TspecItems.Add(item);
            }
            _dbFimsContext.SaveChanges();

            return 1;
        }

        /// <summary>
        ///     1) TestNo=3026(투입량)이 있는 검사 Sheet에서 정보를 뽑아 온다.
        ///     2) TestNo=3009(Coolant종류), 7857 (drain1), 8025 (drain2)값을 가져온다.
        ///     3) s/n, model, 검사시작, 검사완료 정보를 TSheet에서 가져온다.
        ///     
        ///     TestNo=3026이 있는 TSheetNum을 구한다.
        ///     각 TSheetNum에 대하여 3009, 7857, 8025 를 List를 구한다.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public List<CoolantInfo> GetCoolantInfoFromFIMS(DateTime start, DateTime end)
        {
            List<CoolantInfo> coolantInfos = new List<CoolantInfo>();
            List<int> sheets = new List<int>();
            List<Titem> items = new List<Titem>();
            // sheets = _dbFimsContext.Titems.Where(x => x.TestNo == 3026).Select(t => t.TsheetId).ToList();
            sheets = _dbFimsContext.Tsheets.Where(x => x.InspectionEndDateTime >= start && x.InspectionEndDateTime < end).Select(t => t.Id).ToList();
            items = _dbFimsContext.Titems.Where(x => sheets.Contains(x.TsheetId) == true && 
                            (x.TestNo == 3026 || x.TestNo == 3009 || x.TestNo == 7857 || x.TestNo == 8025)).OrderBy(p=>p.TsheetId).ToList();

            for (int i = 0; i < sheets.Count; i++)
            {
                try
                {
                    int idS = 0;
                    if (i == 39)
                    {
                        idS++;
                    }
                    CoolantInfo ci = new CoolantInfo();
                    idS = sheets[i];
                    List<Titem> tmpItems = items.Where(x => x.TsheetId == sheets[i]).ToList();
                    if (tmpItems.Count <= 0)
                        continue;

                    ci.model = _dbFimsContext.Tsheets.FirstOrDefault(x => x.Id == sheets[i]).ProductModel;
                    ci.serial = _dbFimsContext.Tsheets.FirstOrDefault(x => x.Id == sheets[i]).ProductSerial;
                    ci.dateStarted = _dbFimsContext.Tsheets.FirstOrDefault(x => x.Id == sheets[i]).InspectionStartDateTime;
                    ci.dateEnded = _dbFimsContext.Tsheets.FirstOrDefault(x => x.Id == sheets[i]).InspectionEndDateTime;
                    ci.NumCh = tmpItems[0].Channels;
                    ci.coolantname1 = tmpItems.Where(x => x.TestNo == 3009).Select(t => t.Ch1Data).FirstOrDefault();
                    ci.coolantcharge1 = GetFloatValue(tmpItems.Where(x => x.TestNo == 3026).FirstOrDefault(), 1);
                    ci.coolantdrain11 = GetFloatValue(tmpItems.Where(x => x.TestNo == 7857).FirstOrDefault(), 1);
                    ci.coolantdrain12 = GetFloatValue(tmpItems.Where(x => x.TestNo == 8025).FirstOrDefault(), 1);
                    ci.coolantname2 = tmpItems.Where(x => x.TestNo == 3009).Select(t => t.Ch2Data).FirstOrDefault();
                    ci.coolantcharge2 = GetFloatValue(tmpItems.Where(x => x.TestNo == 3026).FirstOrDefault(), 2);
                    ci.coolantdrain21 = GetFloatValue(tmpItems.Where(x => x.TestNo == 7857).FirstOrDefault(), 2);
                    ci.coolantdrain22 = GetFloatValue(tmpItems.Where(x => x.TestNo == 8025).FirstOrDefault(), 2);
                    ci.coolantname3 = tmpItems.Where(x => x.TestNo == 3009).Select(t => t.Ch3Data).FirstOrDefault();
                    ci.coolantcharge3 = GetFloatValue(tmpItems.Where(x => x.TestNo == 3026).FirstOrDefault(), 3);
                    ci.coolantdrain31 = GetFloatValue(tmpItems.Where(x => x.TestNo == 7857).FirstOrDefault(), 3);
                    ci.coolantdrain32 = GetFloatValue(tmpItems.Where(x => x.TestNo == 8025).FirstOrDefault(), 3);
                    ci.coolantname4 = tmpItems.Where(x => x.TestNo == 3009).Select(t => t.Ch4Data).FirstOrDefault();
                    ci.coolantcharge4 = GetFloatValue(tmpItems.Where(x => x.TestNo == 3026).FirstOrDefault(), 4);
                    ci.coolantdrain41 = GetFloatValue(tmpItems.Where(x => x.TestNo == 7857).FirstOrDefault(), 4);
                    ci.coolantdrain42 = GetFloatValue(tmpItems.Where(x => x.TestNo == 8025).FirstOrDefault(), 4);
                    coolantInfos.Add(ci);
                }
                catch (Exception ex)
                {
                    int kk = 0;

                    kk++;
                }
            }
            return coolantInfos;
        }

        private double GetFloatValue(Titem item, int iCh)
        {
            double fValue = 0.0;
            if (item != null)
            {
                string sValue = "";
                if (iCh == 1)
                {
                    sValue = item.Ch1Data;
                } else if (iCh == 2)
                {
                    sValue = item.Ch2Data;
                }
                else if (iCh == 3)
                {
                    sValue = item.Ch3Data;
                }
                else if (iCh == 4)
                {
                    sValue = item.Ch4Data;
                }
                else
                {
                    return fValue;
                }
                fValue = Convert.ToDouble(sValue);
            }

            return fValue;
        }
    }
}
