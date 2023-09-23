#include "Color.h"

ColorRGBA ColorRGBA_Create(int p_r, int p_g, int p_b, int p_a)
{
    assert((0 <= p_r) && (p_r < 256));
    assert((0 <= p_g) && (p_g < 256));
    assert((0 <= p_b) && (p_b < 256));
    assert((0 <= p_a) && (p_a < 256));

    ColorRGBA res = {
        .m_r = (Uint8)p_r,
        .m_g = (Uint8)p_g,
        .m_b = (Uint8)p_b,
        .m_a = (Uint8)p_a
    };

    return res;
}
