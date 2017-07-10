using Development.Dal.Base;
using NHibernate;

namespace Development.Dal.Common
{
    public class CommonRepository : GenericRepository
    {
        public CommonRepository(ISessionFactory sessionFactory)
            : base(sessionFactory)
        {

        }

    }
}
