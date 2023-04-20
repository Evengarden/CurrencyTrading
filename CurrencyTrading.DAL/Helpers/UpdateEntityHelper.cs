using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyTrading.DAL.Helpers
{
    public static class UpdateEntityHelper
    {
        public static Model updateEntity<DTO,Model>(DTO currentEntity, Model updatedEntity)
        {
            var properties = currentEntity.GetType().GetProperties();

            foreach ( var property in properties ) 
            {
                var updatedValue = updatedEntity.GetType().GetProperty(property.Name).GetValue(updatedEntity, null);
                var currentValue = currentEntity.GetType().GetProperty(property.Name).GetValue(currentEntity, null);
                if (updatedValue != currentValue)
                {
                    updatedEntity.GetType().GetProperty(property.Name).SetValue(updatedEntity, currentValue);
                }
            }

            return updatedEntity;
        }
    }
}
