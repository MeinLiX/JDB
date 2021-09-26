namespace JDBSource.Source
{
    enum FileTypes
    {
        //DBs
        DB_config = 0,
        DB_suffix,

        //Schemes
        Scheme_suffix,

        //Tables
        Table_suffix,
        Table_config
    }


    static class EnumExpansion
    {
        public static string Get(this FileTypes fileType) => fileType switch
        {
            FileTypes.DB_suffix => "_JDB",
            FileTypes.DB_config => ".db_option.db.json",

            FileTypes.Scheme_suffix => "_Scheme",

            FileTypes.Table_suffix => ".db.json",
            FileTypes.Table_config => ".table_option.db.json",

            _ => throw new System.Exception("Unknown FileTypes.")
        };
    }
}
