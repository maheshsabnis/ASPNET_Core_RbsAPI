namespace RbsAPI.Repositories
{
    public class OrdersService  
    {
        OrdersDbContext ctx;
        public OrdersService(OrdersDbContext ctx)
        {
            this.ctx = ctx;
        }

        public async Task<Orders> CreateAsync(Orders entity)
        {
             
            entity.TotalPrice = entity.UnitPrice * entity.Quantity;
            var res = await ctx.Orders.AddAsync(entity);
            await ctx.SaveChangesAsync();
            return res.Entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            bool isDeleted = false;
            var order = await ctx.Orders.FindAsync(id);
            if (order != null)
            {
                ctx.Orders.Remove(order);
                await ctx.SaveChangesAsync();
                isDeleted = true;
            }
            return isDeleted;
        }

        public async Task<IEnumerable<Orders>> GetAsync()
        {
            return await ctx.Orders.ToListAsync();
        }

        public async Task<IEnumerable<Orders>> GetNotApprovedOrdersAsync()
        {
            return await ctx.Orders.Where(ord=>ord.IsOrderApproved == false).ToListAsync();
        }


        public async Task<Orders> GetAsync(int id)
        {
           return  await ctx.Orders.FindAsync(id);
        }

        public async Task<bool> UpdateAsync(int id, Orders entity)
        {
            bool isUpdated = false;
            var order = await ctx.Orders.FindAsync(id);
            // update order if it is not already approved
            if (order != null || !order.IsOrderApproved)
            {
                order.CustomerName = entity.CustomerName;
                order.ItemName = entity.ItemName;
                order.Quantity = entity.Quantity;
                order.TotalPrice = entity.UnitPrice * entity.Quantity;
                order.UpdatedBy = entity.UpdatedBy;
                order.UpdatedDate = entity.UpdatedDate;
                await ctx.SaveChangesAsync();
                isUpdated = true;
            }
            return isUpdated;
        }

        public async Task<bool> ApproveOrderAsync(int id)
        {
            bool isUpdated = false;
            var order = await ctx.Orders.FindAsync(id);
            // update order if it is not already approved then only approve it
            if (order != null || !order.IsOrderApproved)
            {
                order.IsOrderApproved = true;
                await ctx.SaveChangesAsync();
                isUpdated = true;
            }
            return isUpdated;
        }
    }
}
