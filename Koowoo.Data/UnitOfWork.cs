using Koowoo.Data.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koowoo.Data
{
    public class UnitOfWork: IUnitOfWork
    {
        private IDbContext _context;
        private ObjectContext _objectContext;
        private IDbTransaction _transaction;

        private bool _disposed;

        public UnitOfWork(IDbContext context)
        {
            _context = context;
        }

        public void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.Unspecified)
        {
            _objectContext = ((IObjectContextAdapter)_context).ObjectContext;
            if (_objectContext.Connection.State != ConnectionState.Open)
                _objectContext.Connection.Open();

            _transaction = _objectContext.Connection.BeginTransaction(isolationLevel);
        }

        public void Commit()
        {
            _transaction.Commit();
        }

        public void Rollback()
        {
            _transaction.Rollback();
            foreach (var entry in _context.CurrentEntries())
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Unchanged;
                        break;
                }
            }
        }

        public void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                try
                {
                    if (_objectContext != null && _objectContext.Connection.State == ConnectionState.Open)
                        _objectContext.Connection.Close();
                }
                catch (ObjectDisposedException)
                {
                }
                if (_context != null)
                {
                    _context.Dispose();
                    _context = null;
                }
            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
