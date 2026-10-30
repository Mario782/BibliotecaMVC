-- 1. Quitar los roles asignados al correo admin
DELETE FROM AspNetUserRoles 
WHERE UserId IN (SELECT Id FROM AspNetUsers WHERE Email = 'admin@biblioteca.com');

-- 2. Eliminar el usuario admin viejo
DELETE FROM AspNetUsers 
WHERE Email = 'admin@biblioteca.com';