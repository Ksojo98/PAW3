"""
Generates a SQL seed file with 1,000,000 product rows using Faker.

Usage:
    pip install faker
    python generate_products_seed.py
"""

import random
from faker import Faker

fake = Faker()

TOTAL_ROWS = 1_000_000
BATCH_SIZE = 1_000  # SQL Server max rows per INSERT VALUES
OUTPUT_FILE = r"C:\Users\Cuent\Downloads\003-ProductsSeed.sql"

ADJECTIVES = [
    "Premium", "Organic", "Ultra", "Classic", "Deluxe", "Essential",
    "Professional", "Eco-Friendly", "Advanced", "Natural", "Compact",
    "Portable", "Smart", "Wireless", "Heavy-Duty", "Lightweight",
    "Vintage", "Modern", "Artisan", "Handcrafted", "Industrial",
    "Luxury", "Elite", "Fresh", "Pure", "Golden", "Crystal", "Turbo",
    "Flex", "Max", "Pro", "Mini", "Mega", "Supreme", "Royal",
]

PRODUCT_TYPES = [
    "Headphones", "Keyboard", "Mouse", "Monitor", "Laptop Stand",
    "Desk Lamp", "Backpack", "Water Bottle", "Coffee Mug", "Notebook",
    "Charger", "USB Hub", "Webcam", "Microphone", "Speaker",
    "Sneakers", "T-Shirt", "Jacket", "Sunglasses", "Watch",
    "Blender", "Toaster", "Air Fryer", "Vacuum Cleaner", "Iron",
    "Shampoo", "Conditioner", "Face Cream", "Lip Balm", "Perfume",
    "Yoga Mat", "Dumbbell Set", "Resistance Band", "Jump Rope",
    "Running Shoes", "Protein Powder", "Vitamins", "Tea Set",
    "Candle", "Plant Pot", "Throw Pillow", "Wall Clock", "Rug",
    "Cutting Board", "Knife Set", "Pan", "Skillet", "Spatula",
    "Tablet", "Phone Case", "Screen Protector", "Power Bank",
    "Drone", "Action Camera", "Tripod", "Ring Light", "SD Card",
    "Guitar Strings", "Drum Sticks", "Piano Bench", "Violin Bow",
]

BRANDS = [
    "NovaTech", "Zenith", "Apex", "Orion", "Pinnacle", "Vortex",
    "Summit", "Solstice", "Aurora", "Horizon", "Cascade", "Ember",
    "Nimbus", "Prism", "Velvet", "Obsidian", "Sapphire", "Titan",
    "Luna", "Phoenix", "Granite", "Breeze", "Pulse", "Onyx",
    "Crestline", "Vertex", "Harbor", "Crescent", "Ridgeway",
]

DESC_TEMPLATES = [
    "High-quality {product} by {brand}. {sentence}",
    "{brand} {product} — designed for everyday comfort and performance. {sentence}",
    "The {brand} {product} delivers exceptional value. {sentence}",
    "Experience the {adj} {product} from {brand}. {sentence}",
    "Upgrade your routine with the {brand} {adj} {product}. {sentence}",
    "{adj} {product} built with care by {brand}. {sentence}",
]


def generate_product_name():
    adj = random.choice(ADJECTIVES)
    product = random.choice(PRODUCT_TYPES)
    brand = random.choice(BRANDS)
    return f"{brand} {adj} {product}"


def generate_description():
    template = random.choice(DESC_TEMPLATES)
    return template.format(
        product=random.choice(PRODUCT_TYPES),
        brand=random.choice(BRANDS),
        adj=random.choice(ADJECTIVES),
        sentence=fake.sentence(nb_words=random.randint(8, 15)),
    )


def sql_escape(text: str) -> str:
    return text.replace("'", "''")


def main():
    print(f"Generating {TOTAL_ROWS:,} product rows...")

    with open(OUTPUT_FILE, "w", encoding="utf-8") as f:
        f.write("-- Auto-generated product seed: 1,000,000 rows\n")
        f.write("-- Generated with Faker + Python\n\n")
        f.write("USE ProductDB;\nGO\n\n")
        f.write("SET NOCOUNT ON;\n\n")

        for batch_start in range(0, TOTAL_ROWS, BATCH_SIZE):
            batch_end = min(batch_start + BATCH_SIZE, TOTAL_ROWS)

            f.write("INSERT INTO Products (\n")
            f.write("    ProductName, InventoryID, SupplierID, Description,\n")
            f.write("    Rating, CategoryID, LastModified, ModifiedBy, CreatedBy\n")
            f.write(") VALUES\n")

            rows = []
            for i in range(batch_start, batch_end):
                name = sql_escape(generate_product_name())
                desc = sql_escape(generate_description())
                inventory_id = (i % 10) + 1
                supplier_id = (i % 5) + 1
                rating = round(random.uniform(1.0, 5.0), 2)
                category_id = (i % 5) + 1

                row = (
                    f"(N'{name}', {inventory_id}, {supplier_id}, "
                    f"N'{desc}', {rating}, {category_id}, "
                    f"GETDATE(), 'seed-script', 'seed-script')"
                )
                rows.append(row)

            f.write(",\n".join(rows))
            f.write(";\nGO\n\n")

            if (batch_start // BATCH_SIZE) % 100 == 0:
                done = batch_end
                print(f"  {done:>10,} / {TOTAL_ROWS:,} rows written...")

    print(f"Done! Output: {OUTPUT_FILE}")


if __name__ == "__main__":
    main()