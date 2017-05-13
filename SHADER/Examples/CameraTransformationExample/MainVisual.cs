﻿using DMS.Application;
using DMS.Base;
using DMS.Geometry;
using DMS.OpenGL;
using DMS.ShaderDebugging;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Diagnostics;
using System.IO;

namespace Example
{
	public class MainVisual
	{
		public MainVisual()
		{
			var dir = Path.GetDirectoryName(PathTools.GetSourceFilePath()) + @"\Resources\";
			shaderWatcher = new ShaderFileDebugger(dir + "vertex.glsl", dir + "fragment.glsl"
				, Resourcen.vertex, Resourcen.fragment);

			CameraDistance = 10.0f;

			GL.Enable(EnableCap.DepthTest);
			GL.Enable(EnableCap.CullFace);
			timeSource.Start();
		}

		public float CameraDistance { get; set; }
		public float CameraAzimuth { get; set; }
		public float CameraElevation { get; set; }

		public void Render()
		{
			if (shaderWatcher.CheckForShaderChange())
			{
				//update geometry when shader changes
				UpdateGeometry(shaderWatcher.Shader);
			}
			var shader = shaderWatcher.Shader;
			var time = (float)timeSource.Elapsed.TotalSeconds;
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			shader.Activate();
			GL.UniformMatrix4(shader.GetUniformLocation("camera"), true, ref camera);
			geometry.Draw(particelCount);
			shader.Deactivate();
		}

		private const int particelCount = 500;
		private ShaderFileDebugger shaderWatcher;
		private Stopwatch timeSource = new Stopwatch();
		private Matrix4 camera = Matrix4.Identity;
		private VAO geometry;

		private void UpdateGeometry(Shader shader)
		{
			Mesh mesh = Obj2Mesh.FromObj(Resourcen.suzanne);
			geometry = VAOLoader.FromMesh(mesh, shader);

			//per instance attributes
			var rnd = new Random(12);
			Func<float> Rnd01 = () => (float)rnd.NextDouble();
			Func<float> RndCoord = () => (Rnd01() - 0.5f) * 35.0f;
			var instancePositions = new Vector3[particelCount];
			for (int i = 0; i < particelCount; ++i)
			{
				instancePositions[i] = new Vector3(RndCoord(), RndCoord(), RndCoord());
			}
			geometry.SetAttribute(shader.GetAttributeLocation("instancePosition"), instancePositions, VertexAttribPointerType.Float, 3, true);
		}

		public void Update(float updatePeriod)
		{
			//todo student: use CameraDistance, CameraAzimuth, CameraElevation
			var p = Matrix4.Transpose(Matrix4.CreatePerspectiveFieldOfView(0.5f, 1.0f, 0.1f, 100.0f));
			camera = p;
		}
	}
}
