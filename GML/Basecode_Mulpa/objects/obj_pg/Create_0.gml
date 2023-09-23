/// Population.
m_population_size = 100;
m_population = array_create(m_population_size, -1);
m_population_cursor = 0;

/// Individus (+enfants x1/2).
m_selection_size = 20;
m_selection = array_create(m_selection_size, -1);

/// Joueur (IA) actuel qui joue.
m_player = noone;

m_gen = 0;

if (global.ai == false)
{
	instance_create_layer(80, 38, "Objects", obj_player);
	instance_destroy();
}

function population_get_min_index()
{
	var _index_min = -1;
	
	for (var _i = 0; _i < m_population_size; _i++)
	{
		if (m_population[_i] == -1)
		{
			continue;
		}
		
		if (_index_min == -1)
		{
			_index_min = _i;;
			continue;
		}
		
		var _nn_current = m_population[_i];
		var _nn_minimum = m_population[_index_min];
		
		if (NN_GetScore(_nn_current) < NN_GetScore(_nn_minimum))
		{
			_index_min = _i;
		}
	}
	
	return _index_min;
}
