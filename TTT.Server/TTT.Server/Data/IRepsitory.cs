using System.Linq;

namespace TTT.Server.Data {
    public interface IRepsitory<G> where G : class {
        void Add(G entity);
        void Update(G entity);
        G Get(string id);
        IQueryable<G> GetQuerry();

        ushort GetTotalCount();
        void Delete(string id);
    }
}
