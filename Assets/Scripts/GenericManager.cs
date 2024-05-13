/// <summary>
/// 변수 스왑 제네릭
/// </summary>
public class GenericManager<T>
{
    public void Swap (ref T x,ref T y)
    {
        var save = x;
        x = y;
        y = save;
    }
}
