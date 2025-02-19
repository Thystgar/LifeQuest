import React, { useEffect, useState } from 'react';
import { View, Text, StyleSheet } from 'react-native';

interface Account {
	name: string;
	group: string;
	points: number;
  }
  
export default function Header() {
	const [accountInfo, setAccountInfo] = useState<Account>({ name: '', group: '', points: 0 });

	useEffect(() => {
	  const fetchAccountInfo = async () => {
		try {
		  const response = await fetch('http://10.0.2.2:8080/account');
		  if (!response.ok) {
			throw new Error(`HTTP error! status: ${response.status}`);
		  }
		  const data = await response.json();
		  setAccountInfo(data);
		} catch (error) {
		  console.error('Error fetching account information:', error);
		}
	  };
  
	  fetchAccountInfo();
	}, []);
  
	return (
	  <View style={styles.header}>
		{accountInfo && (
		  <View style={styles.accountInfoContainer}>
			<Text style={{ flex: 1 }}>{accountInfo.name}</Text>
			<Text style={{ flex: 1 }}>{accountInfo.group}</Text>
			<Text style={{ textAlign: 'right' }}>{accountInfo.points} points</Text>
		  </View>
		)}
	  </View>
	);
}

const styles = StyleSheet.create({
	header: {
		height: 60,
		backgroundColor: '#f8f8f8',
		justifyContent: 'center',
		alignItems: 'center',
		borderBottomWidth: 1,
		borderBottomColor: '#e8e8e8',
	  },
	  headerText: {
		fontSize: 20,
		fontWeight: 'bold',
	  },
	  accountInfoContainer: {
		flexDirection: 'row',
		alignItems: 'center',
	  },
});