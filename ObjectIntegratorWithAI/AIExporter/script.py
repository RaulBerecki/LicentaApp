import bpy
import random

# Create a pyramid
bpy.ops.mesh.primitive_cone_add(vertices=4, radius1=1, depth=1)

# Select the pyramid object
obj = bpy.context.object

# Apply random colors to the pyramid
color = (random.random(), random.random(), random.random(), 1)
obj.data.materials.append(bpy.data.materials.new(name="Color"))
obj.data.materials[0].diffuse_color = color

# Export the pyramid as FBX
bpy.ops.export_scene.fbx(filepath="D:/UnityProjects/LicentaApp/ObjectIntegratorWithAI/AIExporter/pyramid.fbx")