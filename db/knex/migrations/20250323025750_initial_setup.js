exports.up = async function (knex) {
    await knex.raw(`
        CREATE EXTENSION IF NOT EXISTS "uuid-ossp";
        CREATE TABLE users(  
            user_id text NOT NULL PRIMARY KEY DEFAULT uuid_generate_v4(),
            username text NOT NULL UNIQUE,
            password_hash text NOT NULL,
            status text DEFAULT 'anonymous' CHECK (status IN ('admin', 'premium', 'anonymous')),
            created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
            updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
        );

        CREATE TABLE plugins(
            plugin_id text NOT NULL PRIMARY KEY,
            name text NOT NULL,
            description text,
            category text,
            is_enabled boolean DEFAULT TRUE,
            is_premium boolean DEFAULT FALSE,
            created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
            updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
        );

        CREATE TABLE user_starred_plugins(
            user_id text NOT NULL REFERENCES users(user_id) ON DELETE CASCADE,
            plugin_id text NOT NULL REFERENCES plugins(plugin_id) ON DELETE CASCADE,
            PRIMARY KEY (user_id, plugin_id)
        );

        COMMENT ON TABLE users IS 'User account information';
        COMMENT ON TABLE plugins IS 'Plugin information';
        COMMENT ON TABLE user_starred_plugins IS 'Junction table tracking which plugins users have starred';
    `);
};

exports.down = async function (knex) {
    await knex.raw(`
        DROP TABLE user_starred_plugins;
        DROP TABLE plugins;
        DROP TABLE users;
    `);
};
