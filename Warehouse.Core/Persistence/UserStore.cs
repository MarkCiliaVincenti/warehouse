﻿using Microsoft.EntityFrameworkCore;
using Warehouse.Core.Entities.Models;
using Warehouse.Core.Entities.Models.Security;

namespace Warehouse.Core.Persistence
{
    public class UserStore : IUserStore<UserEntity>, IUserRoleStore
    {
        private readonly WarehouseContext _context;

        public UserStore(WarehouseContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<SecurityRoleEntity>> GetRolesAsync(IEnumerable<object> providers, CancellationToken cancellationToken)
        {
            var rl = await EmbeddedRolesAsync();
            var roles = await _context.Set<SecurityRoleEntity>()
                .Where(r => r.ProviderId != null && providers.Contains(r.ProviderId.Value)).ToListAsync(cancellationToken: cancellationToken);
            rl.AddRange(roles);
            return rl;
        }

        public Task<List<SecurityRoleEntity>> EmbeddedRolesAsync()
        {
            return _context.Set<SecurityRoleEntity>().Where(r => r.ProviderId == null).ToListAsync();
        }

        public Task<SecurityRoleEntity> GetRoleAsync(string roleId)
        {
            return _context.Set<SecurityRoleEntity>().FirstOrDefaultAsync(r => r.Id == roleId);
        }

        public Task<List<SecurityObjectEntity>> GetObjectsAsync()
        {
            return _context.Set<SecurityObjectEntity>().ToListAsync();
        }

        public Task<List<RolePermissionsDTO>> GetRolePermissionsAsync(string roleId)
        {
            var query = from rp in _context.Set<SecurityRolePermissionsEntity>()
                join so in _context.Set<SecurityObjectEntity>() on rp.ObjectId equals so.Id
                where rp.RoleId == roleId
                select new RolePermissionsDTO
                {
                    Id = rp.Id,
                    RoleId = rp.RoleId,
                    ObjectId = rp.ObjectId,
                    ObjectName = so.Name,
                    Permissions = rp.Permissions
                };

            return query.ToListAsync();
        }

        public Task<List<RoleDTO>> GetUserRolesAsync(object userId, CancellationToken cancellationToken = default)
        {
            var query = (from ur in _context.Set<UserRoleEntity>()
                join r in _context.Set<SecurityRoleEntity>() on ur.RoleId equals r.Id
                where ur.UserId.Equals(userId)
                select new RoleDTO
                {
                    Id = r.Id,
                    Name = r.Name,
                    Description = r.Description,
                    Items = (from rp in _context.Set<SecurityRolePermissionsEntity>()
                        join so in _context.Set<SecurityObjectEntity>() on rp.ObjectId equals so.Id
                        where rp.RoleId == r.Id
                        select new RolePermissionsDTO
                        {
                            Id = rp.Id,
                            RoleId = rp.RoleId,
                            ObjectId = rp.ObjectId,
                            ObjectName = so.Name,
                            Permissions = rp.Permissions
                        }).ToList()
                });

            return query.ToListAsync(cancellationToken: cancellationToken);
        }

        public Task<UserEntity> FindByIdAsync(object userId, CancellationToken cancellationToken)
        {
            return _context
                .Users
                .Include(u => u.RefreshTokens)
                .AsNoTrackingWithIdentityResolution()
                .SingleOrDefaultAsync(u => u.Id.Equals(userId), cancellationToken: cancellationToken);
        }

        public Task<UserEntity> FindByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
        {
            return _context
                .Users
                .Include(u => u.RefreshTokens)
                .AsTracking()
                .SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == refreshToken), cancellationToken: cancellationToken);
        }

        public Task<UserEntity> FindByNameAsync(string username, CancellationToken cancellationToken)
        {
            return _context
                    .Set<UserEntity>()
                    .Include(u => u.RefreshTokens)
                    .AsTracking()
                    .SingleOrDefaultAsync(u => u.Username == username, cancellationToken: cancellationToken);
        }

        public async Task UpdateAsync(IUser user, CancellationToken cancellationToken)
        {
            //_context.Update(user);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
