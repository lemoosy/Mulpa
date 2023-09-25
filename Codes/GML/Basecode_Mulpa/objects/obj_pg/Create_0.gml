m_x_start = 16;
m_y_start = 96;

m_population_size = 100;
m_population = array_create(m_population_size, -1);

m_selection_size = 20;
m_selection = array_create(m_selection_size, -1);

m_children_size = 40;
m_children = array_create(m_children_size, -1);

m_player = noone;

m_gen = 0;

if (global.ai == false)
{
	instance_create_layer(m_x_start, m_y_start, "Objects", obj_player);
	instance_destroy();
}

function population_insert(_nn_id)
{
	for (var _i = 0; _i < m_population_size; _i++)
	{
		if (m_population[_i] == -1)
		{
			m_population[_i] = _nn_id;
			return;
		}
	}
	
	assert("ERROR - population_insert()");
}

function population_remove_minimum()
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
			_index_min = _i;
			continue;
		}
		
		var _nn_current = m_population[_i];
		var _nn_minimum = m_population[_index_min];
		
		if (NN_GetScore(_nn_current) < NN_GetScore(_nn_minimum))
		{
			_index_min = _i;
		}
	}
	
	var _res = m_population[_index_min];
	
	m_population[_index_min] = -1;
	
	return _res;
}

function population_destroy()
{
	for (var _i = 0; _i < m_population_size; _i++)
	{
		if (m_population[_i] != -1)
		{
			NN_Destroy(m_population[_i]);
			m_population[_i] = -1;
		}
	}
}

function population_is_full()
{
	for (var _i = 0; _i < m_population_size; _i++)
	{
		if (m_population[_i] == -1)
		{
			return false;
		}
	}
	
	return true;
}

function selection_insert(_nn_id)
{
	for (var _i = 0; _i < m_selection_size; _i++)
	{
		if (m_selection[_i] == -1)
		{
			m_selection[_i] = _nn_id;
			return;
		}
	}
	
	assert("ERROR - selection_insert()");
}

function selection_remove_first()
{
	for (var _i = 0; _i < m_selection_size; _i++)
	{
		if (m_selection[_i] != -1)
		{
			var _res = m_selection[_i];
			
			m_selection[_i] = -1;
			
			return _res;
		}
	}
	
	assert("ERROR - selection_remove_first()");
}

function selection_to_population()
{
	for (var _i = 0; _i < m_selection_size; _i++)
	{
		if (m_selection[_i] != -1)
		{
			population_insert(m_selection[_i]);
			m_selection[_i] = -1;
		}
	}
}

function children_insert(_nn_id)
{
	for (var _i = 0; _i < m_children_size; _i++)
	{
		if (m_children[_i] == -1)
		{
			m_children[_i] = _nn_id;
			return;
		}
	}
}

function children_pop()
{
	for (var _i = 0; _i < m_children_size; _i++)
	{
		if (m_children[_i] != -1)
		{
			var _res = m_children[_i];
			m_children[_i] = -1;
			return _res;
		}
	}
	
	assert("ERROR - children_pop()");
}

function children_is_empty()
{
	for (var _i = 0; _i < m_children_size; _i++)
	{
		if (m_children[_i] != -1)
		{
			return false;
		}
	}
	
	return true;
}
