const baseUrl = 'http://localhost:5000/api';

async function postData(endpoint, data) {
    const response = await fetch(`${baseUrl}/${endpoint}`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(data)
    });
    if (!response.ok) {
        const errorText = await response.text();
        throw new Error(`Failed to POST to ${endpoint}: ${response.status} ${response.statusText} - ${errorText}`);
    }
    return await response.json();
}

async function seed() {
    try {
        console.log('Seeding Rooms...');
        const room1 = await postData('Room', {
            name: "Standard Room",
            type: 0, // Standard
            basePrice: 5000.00,
            roomCount: 10,
            availableCount: 10,
            description: "A comfortable standard room.",
            tag: "Standard",
            imageUrl: "https://example.com/standard.jpg",
            isActive: true,
            deleted: false,
            gallery: [],
            reviews: [],
            bookings: [],
            amenities: [],
            priceModifiers: []
        });

        const room2 = await postData('Room', {
            name: "Deluxe Suite",
            type: 1, // Deluxe
            basePrice: 8500.00,
            roomCount: 5,
            availableCount: 5,
            description: "A luxurious suite with extra amenities.",
            tag: "Deluxe",
            imageUrl: "https://example.com/deluxe.jpg",
            isActive: true,
            deleted: false,
            gallery: [],
            reviews: [],
            bookings: [],
            amenities: [],
            priceModifiers: []
        });

        console.log('Seeding Room Galleries...');
        await postData('RoomGallery', { roomId: room1.id, imageUrl: "https://example.com/standard_gallery1.jpg", category: 0, room: null });
        await postData('RoomGallery', { roomId: room2.id, imageUrl: "https://example.com/deluxe_gallery1.jpg", category: 1, room: null });

        console.log('Seeding Amenities...');
        await postData('Amenity', { roomId: room1.id, amenityName: "Free WiFi", icon: "wifi", room: null });
        await postData('Amenity', { roomId: room2.id, amenityName: "Free WiFi", icon: "wifi", room: null });
        await postData('Amenity', { roomId: room2.id, amenityName: "Mini Fridge", icon: "fridge", room: null });

        console.log('Seeding RoomPriceModifiers...');
        await postData('RoomPriceModifier', {
            roomId: room1.id,
            rateName: "Holiday Surge",
            startDate: new Date("2026-12-20").toISOString(),
            endDate: new Date("2026-12-31").toISOString(),
            priceType: 0, // Percentage
            value: 20.0,
            priority: 1,
            room: null
        });
        await postData('RoomPriceModifier', {
            roomId: room2.id,
            rateName: "Weekend Fixed Increase",
            startDate: new Date("2026-08-01").toISOString(),
            endDate: new Date("2026-08-02").toISOString(),
            priceType: 1, // Fixed
            value: 1000.0,
            priority: 2,
            room: null
        });

        console.log('Seeding MenuItems...');
        const menuItem1 = await postData('MenuItem', {
            name: "Masala Dosa",
            description: "Crispy rice crepe with potato filling",
            price: 150.00,
            category: "Main Course",
            isVeg: true,
            imageUrl: "https://example.com/dosa.jpg",
            pantryOrderItems: []
        });
        const menuItem2 = await postData('MenuItem', {
            name: "Cold Coffee",
            description: "Refreshing iced coffee",
            price: 120.00,
            category: "Beverages",
            isVeg: true,
            imageUrl: "https://example.com/coffee.jpg",
            pantryOrderItems: []
        });

        console.log('Seeding CorporateEnquiry...');
        await postData('CorporateEnquiry', {
            name: "John Doe",
            companyName: "Acme Corp",
            email: "john@acme.com",
            phone: "1234567890",
            city: "Delhi",
            requirements: "Need 5 rooms for a conference.",
            message: "Please provide a quote."
        });

        console.log('Seeding Bookings...');
        const booking1 = await postData('Booking', {
            roomId: room1.id,
            bookingReference: "BKG-001",
            checkInDate: new Date("2026-08-10").toISOString(),
            checkOutDate: new Date("2026-08-12").toISOString(),
            guestCount: 2,
            roomCount: 1,
            totalAmount: 10000.00,
            status: 1, // Confirmed
            paymentStatus: 0, // Pending
            guestName: "Alice Smith",
            guestPhone: "9876543210",
            guestEmail: "alice@example.com",
            pantryOrders: [],
            room: null
        });

        console.log('Seeding PantryOrders...');
        const pantryOrder1 = await postData('PantryOrder', {
            bookingId: booking1.id,
            totalAmount: 270.00,
            status: 0, // Received
            orderDate: new Date().toISOString(),
            items: [],
            booking: null
        });

        console.log('Seeding PantryOrderItems...');
        await postData('PantryOrderItem', {
            pantryOrderId: pantryOrder1.id,
            menuItemId: menuItem1.id,
            quantity: 1,
            priceAtTime: 150.00,
            pantryOrder: null,
            menuItem: null
        });
        await postData('PantryOrderItem', {
            pantryOrderId: pantryOrder1.id,
            menuItemId: menuItem2.id,
            quantity: 1,
            priceAtTime: 120.00,
            pantryOrder: null,
            menuItem: null
        });

        console.log('Seeding Reviews...');
        await postData('Review', {
            roomId: room1.id,
            guestName: "Bob Jones",
            rating: 4.5,
            reviewText: "Great stay, very comfortable.",
            reviewDate: new Date().toISOString(),
            room: null
        });

        console.log('Seeding Complete!');
    } catch (error) {
        console.error('Error during seeding:', error);
    }
}

seed();
