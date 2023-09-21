draw_self();

flag = phy_debug_render_aabb | phy_debug_render_collision_pairs | phy_debug_render_obb;
physics_world_draw_debug(flag);