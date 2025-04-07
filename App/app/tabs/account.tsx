import React, { useEffect, useState } from 'react';
import { View, Text, StyleSheet } from 'react-native';
import { fetchAccountInfo } from '../shared/api';
import { Account } from '../shared/types';

export default function Account() {
	const [accountInfo, setAccountInfo] = useState<Account>({ name: '', points: 0 });

	useEffect(() => {
		const fetchData = async () => {
			try {
				const data = await fetchAccountInfo();
				setAccountInfo(data);
			} catch (error) {
				console.error('Error fetching account information:', error);
			}
		};

		fetchData();
	}, []);

	return (
		<View style={styles.container}>
			<View style={styles.listItem}>
				<Text style={styles.key}>Name</Text>
				<Text style={styles.value}>{accountInfo.name}</Text>
			</View>
			<View style={styles.listItem}>
				<Text style={styles.key}>Points</Text>
				<Text style={styles.value}>{accountInfo.points} points</Text>
			</View>
			<View style={styles.listItem}>
				<Text style={styles.key}>Group</Text>
				<Text style={styles.value}>Komarci</Text>
			</View>
		</View>
	);
}

const styles = StyleSheet.create({
	container: {
		flex: 1,
		backgroundColor: '#f9f9f9', // Light background for a professional look
		padding: 20, // Add padding around the container
	},
	listItem: {
		flexDirection: 'row',
		paddingVertical: 15, // Increase padding for better spacing
		borderBottomWidth: 1,
		borderBottomColor: '#ddd', // Subtle border color for separation
	},
	key: {
		fontWeight: '600', // Semi-bold for a professional look
		fontSize: 16,
		width: 120, // Adjust width for better alignment
		marginLeft: 10, // Space on the left of the key
		color: '#333', // Darker text color for better readability
	},
	value: {
		fontSize: 16,
		textAlign: 'left',
		flex: 1,
		marginLeft: 10, // Space between key and value
		color: '#555', // Slightly lighter text color for contrast
	},
});