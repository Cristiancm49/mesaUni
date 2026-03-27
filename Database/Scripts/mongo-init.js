// ===========================================
// SCRIPT DE INICIALIZACIÓN DE MONGODB
// ===========================================

// Crear base de datos principal
db = db.getSiblingDB('ChairaMongoDB');

// Crear usuario para la aplicación
db.createUser({
  user: 'admin1518',
  pwd: '3124553100Mm',
  roles: [
    {
      role: 'readWrite',
      db: 'ChairaMongoDB'
    }
  ]
});

// Crear colecciones iniciales
db.createCollection('audit_logs');
db.createCollection('system_logs');
db.createCollection('user_sessions');
db.createCollection('notifications');
db.createCollection('file_uploads');

// Crear índices para mejor rendimiento
db.audit_logs.createIndex({ "timestamp": 1 });
db.audit_logs.createIndex({ "userId": 1 });
db.audit_logs.createIndex({ "action": 1 });

db.system_logs.createIndex({ "timestamp": 1 });
db.system_logs.createIndex({ "level": 1 });

db.user_sessions.createIndex({ "userId": 1 });
db.user_sessions.createIndex({ "expiresAt": 1 }, { expireAfterSeconds: 0 });

db.notifications.createIndex({ "userId": 1 });
db.notifications.createIndex({ "createdAt": 1 });
db.notifications.createIndex({ "read": 1 });

db.file_uploads.createIndex({ "userId": 1 });
db.file_uploads.createIndex({ "uploadedAt": 1 });

// Insertar datos de ejemplo para desarrollo
db.audit_logs.insertMany([
  {
    _id: ObjectId(),
    timestamp: new Date(),
    userId: "system",
    action: "database_initialized",
    details: "MongoDB database initialized successfully",
    ipAddress: "127.0.0.1",
    userAgent: "MongoDB Init Script"
  }
]);

db.system_logs.insertMany([
  {
    _id: ObjectId(),
    timestamp: new Date(),
    level: "INFO",
    message: "ChairaMongoDB initialized successfully",
    source: "mongo-init.js",
    details: {
      collections: ["audit_logs", "system_logs", "user_sessions", "notifications", "file_uploads"],
      indexes: "created",
      user: "chaira_user created"
    }
  }
]);

print("✅ MongoDB initialization completed successfully!");
print("📊 Database: ChairaMongoDB");
print("👤 User: admin1518");
print("📁 Collections: audit_logs, system_logs, user_sessions, notifications, file_uploads");
print("🔍 Indexes: Created for optimal performance");
