using Microsoft.EntityFrameworkCore;
using ERXTest.Server.DataAccess;
using ERXTest.Server.Helper;
using ERXTest.Server.Services;
using ERXTest.Shared.Helpers;
using ERXTest.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERXTest.Shared.Models.Request;
using Microsoft.EntityFrameworkCore.Storage;
using ERXTest.Shared.DBModels;
using Microsoft.Data.SqlClient;

namespace ERXTest.Server.Repositories
{
    public interface IDropDownRepository
    {
        Task<ERXTestResults<bool>> Upsert(DropDown dropDown, List<DropDownItem> dropDownItem);
        Task<ERXTestResults<DropDownModel>> DropDownItem(int dropDownId);
        Task<ERXTestResults<List<DropDownModel>>> List();
    }

    public class DropDownRepository : IDropDownRepository
    {
        private readonly ILoggerService _logger;
        private readonly ERXTestContext _db;
        public DateTime now;
        public DropDownRepository(ERXTestContext db, ILoggerService logger)
        {
            _logger = logger;
            _db = db;
            now = DateTime.UtcNow.AddHours(7);
        }


        public async Task<ERXTestResults<bool>> Upsert(DropDown dropDown, List<DropDownItem> dropDownItem)
        {
            IDbContextTransaction tran = null;
            try
            {
                tran = await _db.Database.BeginTransactionAsync();

                if (dropDown.Id > 0)
                {

                    await _db.Database.ExecuteSqlRawAsync(@"
UPDATE DropDown SET Name = @ddName WHERE Id = @ddId",
                        new SqlParameter("@ddId", dropDown.Id),
                        new SqlParameter("@ddName", dropDown.Name));

                    var data = await _db.DropDowns.FromSqlRaw(@"
SELECT TOP 1 * FROM DropDown WHERE Id = @ddId
",
                        new SqlParameter("@ddId", dropDown.Id),
                        new SqlParameter("@ddName", dropDown.Name)).FirstOrDefaultAsync();

                    if (data == null)
                    {
                        return ERXTestResponse.CreateErrorResponse<bool>("Error At Upsert, While update name");
                    }
                    if (data.Name != dropDown.Name)
                    {
                        return ERXTestResponse.CreateErrorResponse<bool>("Error At Upsert, Name not update");
                    }

                }
                else
                {
                    await _db.DropDowns.AddAsync(dropDown);
                    await _db.SaveChangesAsync();
                }


                await _db.Database.ExecuteSqlRawAsync(@"
DELETE DropDownItem WHERE DropDownId = @ddId",
                    new SqlParameter("@ddId", dropDown.Id));


                var deleted = await _db.DropDownItems.FromSqlRaw(@"
SELECT TOP 1 * FROM DropDownItem WHERE DropDownId = @ddId
",
                new SqlParameter("@ddId", dropDown.Id)).FirstOrDefaultAsync();
                if (deleted != null)
                {
                    await tran.RollbackAsync();
                    return ERXTestResponse.CreateErrorResponse<bool>("Error At Upsert, While refresh DropDown Items");
                }
                await _db.SaveChangesAsync();

                dropDownItem.ForEach(f => { f.DropDownId = dropDown.Id; f.Id = 0; });
                await _db.DropDownItems.AddRangeAsync(dropDownItem);

                await _db.SaveChangesAsync();

                await tran.CommitAsync();

                return ERXTestResponse.CreateSuccessResponse(true);
            }
            catch (Exception ex)
            {
                await tran.RollbackAsync();
                return ERXTestResponse.CreateErrorResponse<bool>("Error At Upsert", ex);
            }
        }

        public async Task<ERXTestResults<DropDownModel>> DropDownItem(int dropDownId)
        {
            try
            {
                var data = await _db.DropDowns.Where(w => w.Id == dropDownId)
                     .Select(s => new DropDownModel
                     {
                         Id = s.Id,
                         Name = s.Name,
                         DropDownItems = s.DropDownItems.Select(ss => new DropDownItemModel
                         {
                             Name = ss.Name,
                             Id = ss.Id,
                             DropDownId = ss.DropDownId,
                         }).ToList()
                     })
                     .FirstOrDefaultAsync();
                return ERXTestResponse.CreateSuccessResponse(data);

            }
            catch (Exception ex)
            {
                return ERXTestResponse.CreateErrorResponse<DropDownModel>("Error At DropDownItem", ex);
            }
        }

        public async Task<ERXTestResults<List<DropDownModel>>> List()
        {
            try
            {
                var data = await _db.DropDowns
                     .Select(s => new DropDownModel
                     {
                         Id = s.Id,
                         Name = s.Name
                     })
                     .ToListAsync();
                return ERXTestResponse.CreateSuccessResponse(data);

            }
            catch (Exception ex)
            {
                return ERXTestResponse.CreateErrorResponse<List<DropDownModel>>("Error At List", ex);
            }
        }
    }
}
