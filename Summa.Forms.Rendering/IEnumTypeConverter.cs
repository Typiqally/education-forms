namespace Summa.Forms.Rendering
{
    public interface IEnumTypeConverter<T>
    {
        public string ConvertToString(T type);
        public T FromString(string str);
    }
}