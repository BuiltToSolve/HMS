const baseUrl = 'http://localhost:5000/api';

async function fetchJson(endpoint, options = {}) {
    const response = await fetch(`${baseUrl}/${endpoint}`, options);
    if (!response.ok) {
        const errorText = await response.text();
        throw new Error(`Failed to fetch ${endpoint}: ${response.status} ${response.statusText} - ${errorText}`);
    }
    if (response.status === 204) return null; // NoContent for DELETE
    return await response.json();
}

async function postData(endpoint, data) {
    return fetchJson(endpoint, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(data)
    });
}

async function deleteData(endpoint) {
    return fetchJson(endpoint, { method: 'DELETE' });
}

async function reseed() {
    try {
        console.log('Fetching existing rooms...');
        const existingRooms = await fetchJson('Room');
        console.log(`Found ${existingRooms.length} rooms. Deleting...`);
        for (const room of existingRooms) {
            await deleteData(`Room/${room.id}`);
        }

        console.log('Fetching existing menu items...');
        const existingMenuItems = await fetchJson('MenuItem');
        console.log(`Found ${existingMenuItems.length} menu items. Deleting...`);
        for (const item of existingMenuItems) {
            await deleteData(`MenuItem/${item.id}`);
        }

        console.log('Seeding new rooms from constants.ts...');
        
        // ROOM 1: Kashi Comfort Standard
        const room1 = await postData('Room', {
            name: 'Kashi Comfort Standard',
            type: 0, // Standard
            basePrice: 1899,
            description: 'Clean, cozy, and budget-friendly. Perfect for solo travelers and pilgrims focusing on the spiritual journey.',
            tag: 'Best Value',
            imageUrl: 'https://images.unsplash.com/photo-1611892440504-42a792e24d32?q=80&w=2070&auto=format&fit=crop',
            isActive: true,
            deleted: false,
            gallery: [], reviews: [], bookings: [], amenities: [], priceModifiers: []
        });

        // ROOM 1 Amenities
        const room1Amenities = ['Free Wi-Fi', 'AC', 'Mineral Water', 'City View', 'Room Service'];
        for (const am of room1Amenities) {
            await postData('Amenity', { roomId: room1.id, amenityName: am, icon: 'check', room: null });
        }
        
        // ROOM 1 Gallery
        const room1Gallery = [
            { url: 'https://images.unsplash.com/photo-1611892440504-42a792e24d32?q=80&w=2070&auto=format&fit=crop', category: 0 },
            { url: 'https://images.unsplash.com/photo-1582719508461-905c673771fd?q=80&w=2025&auto=format&fit=crop', category: 0 },
            { url: 'https://images.unsplash.com/photo-1631049307264-da0ec9d70304?q=80&w=2070&auto=format&fit=crop', category: 0 },
            { url: 'https://thearchitectsdiary.com/wp-content/uploads/2024/06/Standard-Bathroom-Size-1-1.jpg', category: 1 },
            { url: 'https://images.unsplash.com/photo-1560185893-a55cbc8c57e8?q=80&w=2070&auto=format&fit=crop', category: 2 }
        ];
        for (const g of room1Gallery) {
            await postData('RoomGallery', { roomId: room1.id, imageUrl: g.url, category: g.category, room: null });
        }

        // ROOM 2: Ganga Royal Deluxe
        const room2 = await postData('Room', {
            name: 'Ganga Royal Deluxe',
            type: 1, // Deluxe
            basePrice: 3499,
            description: 'Experience luxury with premium linens, spacious interiors, and a partial view of the holy Ganges.',
            tag: 'Most Popular',
            imageUrl: 'https://images.unsplash.com/photo-1591088398332-8a7791972843?q=80&w=1974&auto=format&fit=crop',
            isActive: true,
            deleted: false,
            gallery: [], reviews: [], bookings: [], amenities: [], priceModifiers: []
        });

        const room2Amenities = ['Ganga View', 'Premium Linen', 'Smart TV', 'Complimentary Breakfast', 'Mini Fridge', 'Bathtub'];
        for (const am of room2Amenities) {
            await postData('Amenity', { roomId: room2.id, amenityName: am, icon: 'check', room: null });
        }

        const room2Gallery = [
            { url: 'https://images.unsplash.com/photo-1591088398332-8a7791972843?q=80&w=1974&auto=format&fit=crop', category: 0 },
            { url: 'https://images.unsplash.com/photo-1590490360182-c33d57733427?q=80&w=1974&auto=format&fit=crop', category: 0 },
            { url: 'https://images.unsplash.com/photo-1560448204-61dc36dc98c8?q=80&w=2070&auto=format&fit=crop', category: 0 },
            { url: 'https://imgs.search.brave.com/29Oav4WIatc7Pa0Zp62TBe7aEYrj4FBSUtEd0woFmhw/rs:fit:860:0:0:0/g:ce/aHR0cHM6Ly9tZWRp/YS1jZG4udHJpcGFk/dmlzb3IuY29tL21l/ZGlhL3Bob3RvLW8v/MTgvMTcvODYvODkv/cm95YWwtcHJpbmNl/c3MtZGVsdXhlLmpw/Zw', category: 1 },
            { url: 'https://images.unsplash.com/photo-1551882547-ff40c63fe5fa?q=80&w=2070&auto=format&fit=crop', category: 3 },
            { url: 'https://images.unsplash.com/photo-1517248135467-4c7edcad34c4?q=80&w=2070&auto=format&fit=crop', category: 6 }
        ];
        for (const g of room2Gallery) {
            await postData('RoomGallery', { roomId: room2.id, imageUrl: g.url, category: g.category, room: null });
        }

        // ROOM 3: Heritage Family Suite
        const room3 = await postData('Room', {
            name: 'Heritage Family Suite',
            type: 1, // Deluxe
            basePrice: 5999,
            description: 'Spacious suite designed for families, featuring traditional Banarasi decor and a private seating area.',
            tag: 'Luxury',
            imageUrl: 'https://images.unsplash.com/photo-1578683010236-d716f9a3f461?q=80&w=2070&auto=format&fit=crop',
            isActive: true,
            deleted: false,
            gallery: [], reviews: [], bookings: [], amenities: [], priceModifiers: []
        });

        const room3Amenities = ['2 King Beds', 'City View', 'Bathtub', 'Butler Service', 'Lounge Area'];
        for (const am of room3Amenities) {
            await postData('Amenity', { roomId: room3.id, amenityName: am, icon: 'check', room: null });
        }

        const room3Gallery = [
            { url: 'https://images.unsplash.com/photo-1578683010236-d716f9a3f461?q=80&w=2070&auto=format&fit=crop', category: 0 },
            { url: 'https://images.unsplash.com/photo-1595526114035-0d45ed16cfbf?q=80&w=2070&auto=format&fit=crop', category: 0 },
            { url: 'https://images.unsplash.com/photo-1584132967334-10e028bd69f7?q=80&w=2070&auto=format&fit=crop', category: 4 }
        ];
        for (const g of room3Gallery) {
            await postData('RoomGallery', { roomId: room3.id, imageUrl: g.url, category: g.category, room: null });
        }

        // ROOM 4: Pilgrim Solo Stay
        const room4 = await postData('Room', {
            name: 'Pilgrim Solo Stay',
            type: 0, // Standard
            basePrice: 1299,
            description: 'Compact and efficient room designed for the modern backpacker or pilgrim. All essentials included.',
            tag: 'Budget Saver',
            imageUrl: 'https://images.unsplash.com/photo-1631049307264-da0ec9d70304?q=80&w=2070&auto=format&fit=crop',
            isActive: true,
            deleted: false,
            gallery: [], reviews: [], bookings: [], amenities: [], priceModifiers: []
        });

        const room4Amenities = ['Single Bed', 'Free Wi-Fi', 'Shared Lounge Access', 'Locker'];
        for (const am of room4Amenities) {
            await postData('Amenity', { roomId: room4.id, amenityName: am, icon: 'check', room: null });
        }

        const room4Gallery = [
            { url: 'https://images.unsplash.com/photo-1631049307264-da0ec9d70304?q=80&w=2070&auto=format&fit=crop', category: 0 },
            { url: 'https://images.unsplash.com/photo-1598928506311-c55ded91a20c?q=80&w=2070&auto=format&fit=crop', category: 0 }
        ];
        for (const g of room4Gallery) {
            await postData('RoomGallery', { roomId: room4.id, imageUrl: g.url, category: g.category, room: null });
        }

        console.log('Seeding new menu items from constants.ts...');
        const menuItemsData = [
            { name: 'Banarasi Kachori Sabzi', description: 'Crispy deep-fried bread with spicy potato curry.', price: 120, category: 'Snacks', isVeg: true, imageUrl: 'https://cms.patrika.com/wp-content/uploads/2023/06/23/banarasi_kachori_sabji_recipe.png?w=800', pantryOrderItems: [] },
            { name: 'Special Thali', description: 'Paneer, Dal, Seasonal Veg, Rice, Roti, Salad, Sweet.', price: 250, category: 'Main Course', isVeg: true, imageUrl: 'https://cdn.uengage.io/uploads/28289/image-TX04RB-1742797428.jpg', pantryOrderItems: [] },
            { name: 'Kulhad Lassi', description: 'Thick yogurt drink served in clay pot with dry fruits.', price: 90, category: 'Beverages', isVeg: true, imageUrl: 'https://imgs.search.brave.com/LVDJMjaOLPtGeWpILcVQnjm9dwKnwdDTiA126QWjfG8/rs:fit:860:0:0:0/g:ce/aHR0cHM6Ly9pbWcu/ZnJlZXBpay5jb20v/cHJlbWl1bS1waG90/by9sYXNzaS10cmFk/aXRpb25hbC1rdWxo/YWQtY3Vwc18xMTc5/MTMwLTYwMTE5Lmpw/Zz9zZW10PWFpc19o/eWJyaWQmdz03NDAm/cT04MA', pantryOrderItems: [] },
            { name: 'Masala Chai', description: 'Spiced Indian tea infused with ginger and cardamom.', price: 40, category: 'Beverages', isVeg: true, imageUrl: 'https://imgs.search.brave.com/8uEjNNmyb6hUY1hrJiEf4trIHimRym84bDznQ-a44L8/rs:fit:860:0:0:0/g:ce/aHR0cHM6Ly9pLnBp/bmltZy5jb20vb3Jp/Z2luYWxzL2QyL2Y0/LzRlL2QyZjQ0ZTkx/NDk1MzgzNGNkNDc4/NGY4NDk3ZjhiZWU2/LmpwZw', pantryOrderItems: [] },
            { name: 'Chicken Biryani', description: 'Aromatic basmati rice cooked with tender chicken and spices.', price: 350, category: 'Main Course', isVeg: false, imageUrl: 'https://imgs.search.brave.com/A-1wWLiCFyXzoy8HhceJvMyjn51BTrkL9rjSLVIYxP8/rs:fit:500:0:1:0/g:ce/aHR0cHM6Ly9tZWRp/YS5pc3RvY2twaG90/by5jb20vaWQvMTgz/NzQyMjkwOC9waG90/by9jaGlja2VuLWJp/cnlhbmkuanBnP3M9/NjEyeDYxMiZ3PTAm/az0yMCZjPWVjUzJX/NnpBWV9qX2JRNURo/bzJsdzlRMExPS0h4/LVhoNzdoVDU3RnZi/VXc9', pantryOrderItems: [] },
            { name: 'Mutton Galouti Kebab', description: 'Melt-in-mouth minced mutton kebabs seasoned with exotic spices.', price: 450, category: 'Snacks', isVeg: false, imageUrl: 'https://www.bombayfisheries.com/cdn/shop/files/gosht-galawati-kebab.jpg?v=1743699884&width=600', pantryOrderItems: [] }
        ];

        for (const item of menuItemsData) {
            await postData('MenuItem', item);
        }

        console.log('Reseeding complete!');
    } catch (err) {
        console.error(err);
    }
}

reseed();
