using System;
using Pattern_of_life.Data;

namespace Pattern_of_life.Repository
{
    public class SettingsRepository
    {
        private readonly ApplicationDbContext _context;

        public SettingsRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        //لتحديد الحد الأقصى للمسافة التي يعتبرها التطبيق بعيدة بما فيه الكفاية ليُظهرها أو يأخذها بعين الاعتبار في الحسابات.
        public double GetDistanceThreshold()
        {
            // Retrieve the settings record (assuming Id is always 1 for settings)
            var settings = _context.Settings.FirstOrDefault(s => s.Id == 1);

            // If settings record found, return DistanceThreshold, otherwise return 0
            return settings != null ? settings.DistanceThreshold : 0;
        }
        // لتحديث قيمة الحد الأقصى
        public void UpdateDistanceThreshold(int Id)
        {
            var settings = _context.Settings.FirstOrDefault(s => s.Id == 1);

            // If settings record found, update DistanceThreshold
            if (settings != null)
            {
                settings.DistanceThreshold = Id;
                _context.SaveChanges();
            }
            else
            {
                // Handle case when settings record not found
                // This might involve creating a new settings record with default values
                // أو التعامل مع حالة عدم العثور على سجل الإعدادات
                // وهذا قد ينطوي على إنشاء سجل إعدادات جديد بالقيم الافتراضية
            }
        }
    }

}
