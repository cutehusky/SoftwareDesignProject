/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> }
 */
exports.seed = async function (knex) {
    // Deletes ALL existing entries
    await knex("plugins").del();
    await knex("plugins").insert([
        {
            plugin_id: "10bba80b-2976-4bd7-a86f-5e97dce50bf4",
            name: "Hash",
            description: "Hash a string using a variety of algorithms",
            category: "Hash",
            is_enabled: true,
            is_premium: false,
        },
        {
            plugin_id: "b33c175c-94a9-4d92-b6f8-b56c6ad450b3",
            name: "Hash Password",
            description: "Hash a password using a variety of algorithms",
            category: "Hash",
            is_enabled: true,
            is_premium: false,
        },
        {
            plugin_id: "c34af0bf-11ad-4c51-bf2b-87d164bdf257",
            name: "Hash File",
            description: "Hash a file using a variety of algorithms",
            category: "Hash",
            is_enabled: true,
            is_premium: false,
        },
        {
            plugin_id: "fckjs345-3281-4099-9eb1-f52498ae94b0",
            name: "Generate Lorem Ispum",
            description: "Generate lorem ispum",
            category: "String",
            is_enabled: true,
            is_premium: false,
        },
        {
            plugin_id: "37a1eea7-567c-4cac-87eb-cece7dc68d57",
            name: "String Obsfucate",
            description: "Obsfucate a string",
            category: "String",
            is_enabled: true,
            is_premium: false,
        },
        {
            plugin_id: "e1a4c6d4-654e-4c44-b8c3-bd246c87bb71",
            name: "Convert Temperature",
            description:
                "Convert temperature between Celsius, Fahrenheit, etc.",
            category: "Conversion",
            is_enabled: true,
            is_premium: false,
        },
    ]);
};
