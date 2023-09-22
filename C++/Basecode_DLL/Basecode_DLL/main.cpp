#define fn_export extern "C" __declspec (dllexport)

fn_export double add_integers(double a, double b)
{
    return a + b;
}
