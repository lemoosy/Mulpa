physics_world_update_iterations(30);
/// Évite que l'objet fasse des rotations sur lui-même.
phy_fixed_rotation = true;

/// Entrées de l'utilisateur.
m_key_left = false;
m_key_right = false;
m_key_jump = false;

/// Variables pour la physique de l'objet.
m_speed = vec2_set(100, 200);
//m_speed = vec2_scale(m_speed, 1);
m_mass = 50;
m_gravity = 100;
m_friction = 1;

/// États de l'objet.
m_on_ground = false;

/// Items.
m_coin = 0;
m_distance_exit = 0;

m_ai = true;

m_nn = NN_Create();

if (m_nn < 0)
{
	assert("ERROR - NN_Create()");
}
