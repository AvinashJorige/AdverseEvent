using EntityLayer;
using Repository;
using System.Collections.Generic;

namespace ServiceLayer
{
    public class AdverseFormService
    {
        AdverseFormSubmitDB submitDB = null;
        public AdverseFormService()
        {
            submitDB = new AdverseFormSubmitDB();
        }

        public string SaveFormChanges(AdverseDataEntity entity)
        {
            return submitDB.SaveFormChanges(entity);
        }

    }
}
