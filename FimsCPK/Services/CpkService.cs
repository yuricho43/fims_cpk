using FimsCPK.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using static Telerik.Blazor.ThemeConstants;

namespace FimsCPK.Services
{
    public class CpkService
    {
        private readonly FimsDbContext _dbFimsContext;
        static List<string> gStrModels = new List<string>();

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
                Model  = cpkDto.Model,
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
            List<TspecItem> tCpkItemsSL = _dbFimsContext.TspecItems.Where(x => idModel == x.TspecModelId && listTestNo.Contains(x.TestNo)).ToList();
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
            List<CpkItem> tCpkItemsCL = _dbFimsContext.CpkItems.Where(x => x.Model == strModel).ToList();
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

        public List<string> GetCpkModelNames()
        {
            //--- if already extract target models
            if (gStrModels.Count > 0)
                return gStrModels;

            List<CpkItem> tCpkModelList;

            using (var db = new FimsDbContext())
            {
                tCpkModelList = db.CpkItems.ToList();
            }

            var models = from cpkModel in tCpkModelList
                         orderby cpkModel.Model
                         group cpkModel by cpkModel.Model into grp
                         select grp.Key;

            foreach (var item in models)
            {
                gStrModels.Add(item.ToString());
            }

            return gStrModels;
        }

    }
}
