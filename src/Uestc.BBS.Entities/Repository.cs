using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using SqlSugar;

namespace Uestc.BBS.Entities
{
    public class Repository<T>(SqlSugarClient db)
        where T : class, new()
    {
        private readonly SqlSugarClient _db = db;

        /// <summary>
        /// 获取符合表达式的第一个实体
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public T First(Expression<Func<T, bool>> expression) =>
            _db.Queryable<T>().First(expression);

        public Task<T> FirstAsync(
            Expression<Func<T, bool>> expression,
            CancellationToken cancellationToken = default
        ) => _db.Queryable<T>().FirstAsync(expression, cancellationToken);

        /// <summary>
        /// 获取符合表达式的单个实体，如果没有符合的实体，则抛出异常
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public T Single(Expression<Func<T, bool>> expression) =>
            _db.Queryable<T>().Single(expression);

        public Task<T> SingleAsync(Expression<Func<T, bool>> expression) =>
            _db.Queryable<T>().SingleAsync(expression);

        /// <summary>
        /// 获取所有实体
        /// </summary>
        /// <returns></returns>
        public ISugarQueryable<T> GetAll() => _db.Queryable<T>();

        /// <summary>
        /// 获取符合表达式的实体列表
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public ISugarQueryable<T> GetList(Expression<Func<T, bool>> expression) =>
            _db.Queryable<T>().Where(expression);

        /// <summary>
        /// 判断是否存在符合表达式的实体
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public bool Any(Expression<Func<T, bool>> expression) => _db.Queryable<T>().Any(expression);

        public async Task<bool> AnyAsync(
            Expression<Func<T, bool>> expression,
            CancellationToken cancellationToken = default
        ) => await _db.Queryable<T>().AnyAsync(expression, cancellationToken);

        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Add(T entity) => _db.Insertable(entity).ExecuteCommand();

        public Task<int> AddAsync(T entity, CancellationToken cancellationToken = default) =>
            _db.Insertable(entity).ExecuteCommandAsync(cancellationToken);

        /// <summary>
        /// 添加/更新实体
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<int> AddOrUpdateAsync(
            T entity,
            CancellationToken cancellationToken = default
        ) => _db.Storageable(entity).ExecuteCommandAsync(cancellationToken);

        /// <summary>
        /// 批量添加实体
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public int AddList(List<T> entities) => _db.Insertable(entities).ExecuteCommand();

        public Task<int> AddListAsync(
            List<T> entities,
            CancellationToken cancellationToken = default
        ) => _db.Insertable(entities).ExecuteCommandAsync(cancellationToken);

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Delete(T entity) => _db.Deleteable(entity).ExecuteCommand();

        public Task<int> DeleteAsync(T entity, CancellationToken cancellationToken = default) =>
            _db.Deleteable(entity).ExecuteCommandAsync(cancellationToken);

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Update(T entity) => _db.Updateable(entity).ExecuteCommand();

        public Task<int> UpdateAsync(T entity, CancellationToken cancellationToken = default) =>
            _db.Updateable(entity).ExecuteCommandAsync(cancellationToken);

        /// <summary>
        /// 分页获取实体列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<List<T>> GetPageListAsync(
            int page,
            int pageSize,
            Func<ISugarQueryable<T>, ISugarQueryable<T>>? opeartion = null,
            CancellationToken cancellationToken = default
        )
        {
            opeartion ??= (queryable => queryable);
            return opeartion(_db.Queryable<T>()).ToPageListAsync(page, pageSize, cancellationToken);
        }
    }
}
