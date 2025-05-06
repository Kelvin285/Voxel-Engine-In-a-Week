#version 430 core

in vec2 o_uv;
in vec3 o_color;

out vec4 render_color;

uniform sampler2D tex;


void main() {
	vec4 tex_color = texture(tex, o_uv);

	tex_color.xyz *= o_color;

	if (tex_color.w <= 0.01) {
		discard;
	}

	render_color = tex_color;
}