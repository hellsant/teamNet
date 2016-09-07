using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestService.Security
{
    public class AccessToken
    {
        private string id;
        private Guid userId;
        private DateTime expires;
        private DateTime validFrom;

        public AccessToken(Guid userId)
        {
            this.validFrom = DateTime.UtcNow;
            this.expires = validFrom.AddMinutes(5);
            this.userId = userId;
            this.id = Guid.NewGuid().ToString();
        }

        public string Id
        {
            get { return id; }
        }

        public Guid UserId
        {
            get { return userId; }
        }

        public DateTime Expires
        {
            get { return expires; }
        }

        public DateTime ValidFrom
        {
            get { return validFrom; }
        }
    }
}