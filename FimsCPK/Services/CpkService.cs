using FimsCPK.Models;
using Microsoft.EntityFrameworkCore;

namespace FimsCPK.Services
{
    public class CpkService
    {
        private readonly FimsDbContext _dbFimsContext;

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

        public string DeleteCpkItem(string Model, string TestNo)
        {
            //=== Delete POManage
            var cpk = _dbFimsContext.CpkItems.FirstOrDefault(p => p.Model == Model && p.TestNo == TestNo);
            if (cpk is null)
                return "Cpk not found";

            _dbFimsContext.CpkItems.Remove(cpk);
            _dbFimsContext.SaveChanges();

            return "Delete successfully";
        }

    }
}
