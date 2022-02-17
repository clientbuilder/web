using System.Collections.Generic;
using System.Threading.Tasks;
using ClientBuilder.Web.Models;

namespace ClientBuilder.Web.Services;

public interface IApplicationServiceAgent
{
    Task<IEnumerable<ScaffoldModule>> FetchScaffoldModulesAsync();
}