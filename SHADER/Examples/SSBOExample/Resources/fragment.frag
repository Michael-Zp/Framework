#version 430 core

in vec4 colorVertex;

out vec4 color;

void main() {
	color = colorVertex;
}