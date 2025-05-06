#version 430 core

layout(location = 0) in vec3 vert;
layout(location = 1) in vec2 uv;
layout(location = 2) in vec3 normal;
layout(location = 3) in vec3 color;

out vec2 o_uv;
out vec3 o_color;

uniform mat4 proj_view_model;
uniform vec3 sun_dir;


void main() {
	vec4 pos = vec4(vert, 1.0f);
	gl_Position = proj_view_model * pos;

	float sunlight = (dot(sun_dir, normal) + 2.0f) / 3.0f;

	o_uv = uv;
	o_color = sunlight * color;
}