using DocumentFormat.OpenXml.InkML;
using FimsCPK.Data;
using FimsCPK.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
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
                return "There is no such a CpkItem"; ;

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
            return "Update failed"; ;
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
    }
}
