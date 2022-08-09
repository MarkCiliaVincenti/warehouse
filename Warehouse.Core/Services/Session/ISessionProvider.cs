﻿using System.Security.Principal;
using Warehouse.Core.Entities.Models.Security;

namespace Warehouse.Core.Services.Session
{
    public interface ISessionProvider
    {
        IPrincipal User { get; }

        bool IsInitialized();
        Task LoadAsync();
        bool HasRole(string role);
        bool HasAnyRole(IEnumerable<string> roles);
        bool HasPermission(string objName, SecurityPermissions requiredPermissions);

        public T Get<T>(string key) where T : class;
        public void Set<T>(string key, T value) where T : class;
        public Task<T> GetAsync<T>(string key) where T : class;
        public Task SetAsync<T>(string key, T value) where T : class;
        public void SetBoolean(string key, bool value);
        public bool? GetBoolean(string key);
        public void SetDouble(string key, double value);
        public double? GetDouble(string key);
        public void SetInt64(string key, long value);
        public long? GetInt64(string key);

        byte[] this[string index]
        {
            get;
            set;
        }
    }
}
