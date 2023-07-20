using misa.hust._21h.api.Entities;

namespace misa.hust._21h.api.Controllers
{
    internal class PagingData<T>
    {
        public PagingData()
        {
        }

        public List<Department> Data { get; set; }
        public int TotalCount { get; set; }
    }
}