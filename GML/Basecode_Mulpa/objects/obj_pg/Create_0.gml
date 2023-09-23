/// Population.
m_population_size = 10;
m_population = array_create(m_population_size, -1);
m_population_cursor = 0;

/// Individus.
m_selection_size = 5;
m_selection = array_create(m_selection_size, -1);

/// Enfants.
m_children_size = 3;
m_children = array_create(m_children_size, -1);

/// Joueur (IA) actuel qui joue.
m_player = noone;

if (global.ai == false)
{
	instance_create_layer(80, 38, "Objects", obj_player);
	instance_destroy();
}

function population_get_min_index()
{
	var _index_min = 0;
	
	for (var _i = 1; _i < m_population_size; _i++)
	{
		var _nn_current = m_population[_i];
		var _nn_minimum = m_population[_index_min];
		
		if (NN_GetScore(_nn_current) < NN_GetScore(_nn_minimum))
		{
			_index_min = _i;
		}
	}
	
	return _index_min;
}
