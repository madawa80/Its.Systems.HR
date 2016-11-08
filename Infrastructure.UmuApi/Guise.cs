using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.UmuApi
{
    public class Guise
    {

        private IEnumerable<Contact> contact;
        public string EPost { get; set; }
        public IEnumerable<Contact> Contacts
        {

           
            get
            {
            
                return this.contact;
            
            }
            set
            {
                foreach (var contact in value)
                {
                    if (contact.EMail.Contains("@umu.se"))
                    {
                        this.EPost = contact.EMail;
                        break;
                    }
                    
                    if(contact.EMail.Contains("@"))
                    {
                        this.EPost = contact.EMail;
                    }
                }
            }



        }


    }

}
