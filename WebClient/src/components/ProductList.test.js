import React from 'react';
import { render, screen, waitFor } from '@testing-library/react';
import { BrowserRouter } from 'react-router-dom';
import ProductList from './ProductList';
import axios from 'axios';

// Mock axios
jest.mock('axios');

describe('ProductList Component', () => {
    const mockProducts = [
        { id: 1, name: 'Product 1', price: 100, image: 'image1.jpg' },
        { id: 2, name: 'Product 2', price: 200, image: 'image2.jpg' }
    ];

    beforeEach(() => {
        axios.get.mockResolvedValue({ data: mockProducts });
    });

    afterEach(() => {
        jest.clearAllMocks();
    });

    test('renders product list correctly', async () => {
        render(
            <BrowserRouter>
                <ProductList />
            </BrowserRouter>
        );

        // Check loading state
        expect(screen.getByText(/loading/i)).toBeInTheDocument();

        // Wait for products to load
        await waitFor(() => {
            expect(screen.getByText('Product 1')).toBeInTheDocument();
            expect(screen.getByText('Product 2')).toBeInTheDocument();
        });

        // Check if all products are rendered
        const products = screen.getAllByRole('img');
        expect(products).toHaveLength(2);
    });

    test('displays error message when API call fails', async () => {
        axios.get.mockRejectedValue(new Error('Failed to fetch products'));

        render(
            <BrowserRouter>
                <ProductList />
            </BrowserRouter>
        );

        await waitFor(() => {
            expect(screen.getByText(/error/i)).toBeInTheDocument();
        });
    });

    test('displays empty state when no products are available', async () => {
        axios.get.mockResolvedValue({ data: [] });

        render(
            <BrowserRouter>
                <ProductList />
            </BrowserRouter>
        );

        await waitFor(() => {
            expect(screen.getByText(/no products available/i)).toBeInTheDocument();
        });
    });
}); 