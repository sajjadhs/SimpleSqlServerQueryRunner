using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shared;
using System.Data;

namespace SimpleSqlServerEditorWeb.Pages
{
    [BindProperties]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        internal static DataContext? _context;

        public string ConnectionString { get; set; }
        public string[] Tables { get; set; }
        public string[] TableSchema { get; set; }
        public List<string> TableData { get; set; }
        public bool ConnectionStatus { get; set; }
        public string SqlQuery { get; set; }

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }

        public void OnPost()
        {
            _context = new DataContext(ConnectionString);
            ConnectionStatus = _context.OpenConnection();
        }

        public void OnPostTables()
        {
            Tables = _context?.GetTables();
        }

        public void OnGetTable(string tableName)
        {
            TableSchema = _context?.GetTableSchema(tableName);
        }

        public void OnPostQuery()
        {
            TableData = _context?.Run(SqlQuery);
        }
    }
}