using SqlSugar;

namespace api.Model
{
    public class User
    {
        [SugarColumn(IsPrimaryKey = true)]
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string Address { get; set; }
        public int Order { get; set; }
    }
}
