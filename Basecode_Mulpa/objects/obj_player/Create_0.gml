/// Évite que l'objet fasse des rotations sur lui-même.
phy_fixed_rotation = true;

/// Entrées de l'utilisateur.
m_key_left = false;
m_key_right = false;
m_key_jump = false;

/// Variables pour la physique de l'objet.
m_speed = vec2_set(100, 125);

/// États de l'objet.
m_on_ground = false;

/// Items.
m_coin = 0;
m_distance_exit = 0;
