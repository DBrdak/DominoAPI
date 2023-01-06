using Microsoft.EntityFrameworkCore;

namespace UtilityLibrary
{
    public static class Methods
    {
        public static void MapTo(this object source, object destination)
        {
            foreach (var sourceProp in source.GetType().GetProperties())
            {
                foreach (var destinationProp in destination.GetType().GetProperties())
                {
                    var dtoPropValue = sourceProp.GetValue(source);
                    if (dtoPropValue != null && sourceProp.Name == destinationProp.Name)
                    {
                        destinationProp.SetValue(destination, dtoPropValue);
                    }
                }
            }
        }
    }
}