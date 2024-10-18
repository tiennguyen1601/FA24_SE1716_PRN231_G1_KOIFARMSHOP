using KOIFARMSHOP.Data.Base;
using KOIFARMSHOP.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KOIFARMSHOP.Data.Repository
{
    public class StaffRepository : GenericRepository<Staff>
    {
        public StaffRepository() { }
        public StaffRepository(FA24_SE1716_PRN231_G1_KOIFARMSHOPContext context) => _context = context;

        public async Task<Staff> GetStaffByUsername(string username)
        {
            try
            {
                return await _context.Staff.FirstOrDefaultAsync(x => x.Username.Equals(username));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> IsExistedByEmail(string email)
        {
            try
            {
                return await _context.Staff.FirstOrDefaultAsync(x => x.Email.Equals(email)) != null ? true : false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> IsExistedByUsername(string username)
        {
            try
            {
                return await _context.Staff.FirstOrDefaultAsync(x => x.Username.Equals(username)) != null ? true : false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> IsExistedByPhone(string phone)
        {
            try
            {
                return await _context.Staff.FirstOrDefaultAsync(x => x.Phone.Equals(phone)) != null ? true : false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
