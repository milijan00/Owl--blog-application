using BlogApp.Domain;
using System.Collections.Generic;

namespace BlogApp.API.Core
{

    public class ActionUser : IActionUser
    {
        public string Identity => "Anonmymous";

        public IEnumerable<int> AllowedUseCasesIds { get; set; } = new List<int>()
        {
             8
        }; 
        // 8 je za registraciju - po potrebi dodati jos neke

        public string Email => "anonymous@gmail.com";

        int IActionUser.Id => 0;
    }
}
