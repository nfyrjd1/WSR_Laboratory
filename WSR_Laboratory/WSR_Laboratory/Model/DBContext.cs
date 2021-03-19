namespace WSR_Laboratory.Model
{
    public partial class Laboratory
    {
        private static Laboratory Context;

        public static Laboratory GetContext()
        {
            if (Context == null)
            {
                Context = new Laboratory();
            }

            return Context;
        }
    }
}
